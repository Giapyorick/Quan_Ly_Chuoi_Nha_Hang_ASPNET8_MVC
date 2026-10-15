using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Collections.Generic;

namespace GiapDepTrai.Controllers
{
    public class ReservationController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public ReservationController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public void LayDL()
        {
            ViewBag.Reservation_Table = _context.TblTables
                .Select(s => new SelectListItem
                {
                    Value = s.TableId.ToString(),
                    Text = s.TableName
                }).ToList();

            ViewBag.Reservation_MenuItem = _context.TblMenuItems
                .Select(s => new SelectListItem
                {
                    Value = s.MenuItemId.ToString(),
                    Text = s.ItemName
                }).ToList();

            ViewBag.Reservation_Customer = _context.TblCustomers
                .Select(s => new SelectListItem
                {
                    Value = s.CustomerId.ToString(),
                    Text = s.FullName
                }).ToList();
            ViewBag.Reservation_Branch = _context.TblBranches
                .Select(s => new SelectListItem
                {
                    Value = s.BranchId.ToString(),
                    Text = s.BranchName
                }).ToList();
        }

        public IActionResult ChooseBranchReservation()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                ViewBag.ErrorMessage = TempData["ErrorMessage"] = "Bạn chưa đang nhập nên chưa thể chọn bàn";
                return RedirectToAction("Login", "Login", new { returnUrl = currentUrl });
            }
            var branch = _context.TblBranches
            .ToList();

            if (TempData["AlertMessage"] != null)
            {
                ViewBag.AlertMessage = TempData["AlertMessage"];
            }
            return View(branch);
        }
        [HttpPost]
        public IActionResult ChooseBranchReservation(int branchId)
        {
            HttpContext.Session.SetInt32("BranchID", branchId);

            return RedirectToAction("ChooseTableReservation", "Reservation");
        }
        public IActionResult ChooseTableReservation()
        {
            var branchId = HttpContext.Session.GetInt32("BranchID");
            if (branchId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn chi nhánh trước khi chọn bàn.";
                return RedirectToAction("ChooseBranchReservation");
            }

            var customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                TempData["ErrorMessage"] = "Bạn chưa đăng nhập nên chưa thể chọn bàn.";
                return RedirectToAction("Login", "Login", new { returnUrl = currentUrl });
            }

            var table = _context.TblTables.Include(m => m.TableType)
                .Where(m => m.BranchId == branchId && m.Status == "Trỗng").ToList();

            return View(table);
        }


        [HttpGet]
        public IActionResult GetTableInfo(int id)
        {
            var table = _context.TblTables
                .Include(t => t.TblReservations.OrderByDescending(t => t.ReservationId))
                .ThenInclude(c => c.Customer)
                .FirstOrDefault(t => t.TableId == id);

            if (table == null)
                return Content("Không tìm thấy bàn.");

            return PartialView("_TableInfoPartial", table);
        }

        public void SetDefault(int CustomerID, int TableID)
        {
            var Customer = _context.TblCustomers.FirstOrDefault(s => s.CustomerId == CustomerID);
            if (Customer is not null)
            {
                ViewBag.CustomerName = Customer.FullName;
            }
            var table = _context.TblTables.FirstOrDefault(s => s.TableId == TableID);
            if (table is not null)
            {
                ViewBag.TableName = table.TableName;
            }
            ViewBag.TableID = TableID;
            var branchid = HttpContext.Session.GetInt32("BranchID");
            var branch = _context.TblBranches.FirstOrDefault(s => s.BranchId == branchid);
            if (branch is not null)
            {
                ViewBag.BranchName = branch.BranchName;
            }

            var menuItems = _context.TblMenuItems.ToList();
            ViewBag.Order_MenuItemAll = menuItems;
            ViewBag.Order_MenuItem = menuItems.Select(m => new SelectListItem
            {
                Value = m.MenuItemId.ToString(),
                Text = m.ItemName,
                Selected = false
            }).ToList();

            ViewBag.MenuItemDetails = menuItems.ToDictionary(
                m => m.MenuItemId.ToString(),
                m => new
                {
                    Descriptions = m.Descriptions,
                    Price = m.Price,
                    ImageURL = m.ImageUrl
                }
            );
            var categories = _context.TblCategories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();
            ViewBag.Order_Categories = new SelectList(categories, "Value", "Text");
            LayDL();
        }
        [HttpGet]
        public IActionResult CreateReservation(int CustomerID, int TableID)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
            {

                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                ViewBag.ErrorMessage = TempData["ErrorMessage"] = "Bạn chưa đang nhập nên chưa thể đặt bàn";
                return RedirectToAction("Login", "Login", new { returnUrl = currentUrl });
            }
            if (!_context.TblTables.Any(t => t.TableId == TableID))
            {
                return NotFound("Không tìm thấy bàn");
            }

            var cartItems = _context.TblCartItems
                .Include(c => c.MenuItem)
                .Include(d => d.Cart)
                .Where(d => d.Cart.CustomerId == CustomerID)
                .ToList();

            var model = new AddNewReservation
            {
                TableId = TableID,
                CustomerId = CustomerID,
                ReservationTime = DateTime.Now,
            };

            LayDL();
            SetDefault(CustomerID, TableID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(AddNewReservation reservation)
        {
            if (!_context.TblCustomers.Any(c => c.CustomerId == reservation.CustomerId))
            {
                ModelState.AddModelError("CustomerId", "Khách hàng không tồn tại");
            }

            if (!_context.TblTables.Any(t => t.TableId == reservation.TableId))
            {
                ModelState.AddModelError("TableId", "Bàn không tồn tại");
            }

            if (!ModelState.IsValid)
            {
                SetDefault(reservation.CustomerId, reservation.TableId);
                return View(reservation);
            }
            var branchId = HttpContext.Session.GetInt32("BranchID");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newReservation = new TblReservation
                {
                    CustomerId = reservation.CustomerId,
                    TableId = reservation.TableId,
                    ReservationTime = reservation.ReservationTime,
                    NumberOfGuests = reservation.NumberOfGuests,
                    Status = "Chờ xác nhận",
                    Notes = reservation.Notes,
                    BranchId = branchId
                };

                _context.TblReservations.Add(newReservation);
                await _context.SaveChangesAsync();
                var cartItems = _context.TblCartItems
                    .Include(c => c.MenuItem)
                    .Include(d => d.Cart)
                    .Where(d => d.Cart.CustomerId == reservation.CustomerId)
                    .ToList();
                if (reservation.MenuItems != null && reservation.MenuItems.Any())
                {
                    foreach (var menuItem in reservation.MenuItems)
                    {
                        _context.TblReservationMenuItems.Add(new TblReservationMenuItem
                        {
                            ReservationId = newReservation.ReservationId,
                            MenuItemId = menuItem.MenuItemId,
                            Quantity = menuItem.Quantity,
                            Note = menuItem.Note
                        });
                    }
                    await _context.SaveChangesAsync();

                }

                var table = _context.TblTables.FirstOrDefault(s => s.TableId == reservation.TableId);
                if (table != null)
                {
                    table.Status = "Đã được đặt";
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = JavaScriptEncoder.Default.Encode($"Không tìm thấy bàn với TableId = {reservation.TableId}");
                    SetDefault(reservation.CustomerId, reservation.TableId);
                    return View(reservation);
                }

                await transaction.CommitAsync();

                TempData["SuccessMessage"] = JavaScriptEncoder.Default.Encode("Đặt bàn thành công!");
                return RedirectToAction("CreatePayment", "ReservationPayment", new { customerId = newReservation.CustomerId, reservationId = newReservation.ReservationId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "Lỗi khi lưu đặt bàn: " + (ex.InnerException?.Message ?? ex.Message);
                SetDefault(reservation.CustomerId, reservation.TableId);
                return View(reservation);
            }
        }
        [HttpGet]
        public IActionResult List(int page = 1, int pageSize = 10)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerID");

            var query = _context.TblReservations
                .Include(r => r.TblReservationMenuItems).ThenInclude(i => i.MenuItem)
                .Include(r => r.Table)
                .Include(r => r.Branch)
                .Include(r => r.Customer)
                .Where(r => r.Customer.CustomerId == customerId);

            int totalItems = query.Count();
            var reservations = query
                .OrderByDescending(r => r.ReservationTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(reservations);
        }
        
    }
}