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
    public class ReservationCartController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public ReservationCartController(QlChuoiNhaHang1Context context){
            _context = context;
        }
        public void LayDL(){
            ViewBag.Reservation_Table = _context.TblTables
                .Select(s => new SelectListItem {
                    Value = s.TableId.ToString(),
                    Text = s.TableName
                }).ToList();

            ViewBag.Reservation_MenuItem = _context.TblMenuItems
                .Select(s => new SelectListItem {
                    Value = s.MenuItemId.ToString(),
                    Text = s.ItemName
                }).ToList();

            ViewBag.Reservation_Customer = _context.TblCustomers
                .Select(s => new SelectListItem {
                    Value = s.CustomerId.ToString(),
                    Text = s.FullName
                }).ToList();
        }

        public IActionResult ChooseBranch()
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
        public IActionResult ChooseBranch(int branchId)
        {
            HttpContext.Session.SetInt32("BranchID", branchId);
            
            return RedirectToAction("ChooseTable", "ReservationCart");
        }
        public IActionResult ChooseTable()
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
        public IActionResult Create(int CustomerID, int TableID)
        {
            if (!_context.TblTables.Any(t => t.TableId == TableID))
            {
                return NotFound("Không tìm thấy bàn");
            }

            var cartItems = _context.TblCartItems
                .Include(c => c.MenuItem)
                .Include(d => d.Cart)
                .Where(d => d.Cart.CustomerId == CustomerID)
            .Select(c => new AddNewCartItem
            {
                CartId = c.CartId,
                MenuItemId = c.MenuItemId,
                Quantity = c.Quantity,
                Note = c.Note,
                Price = c.MenuItem != null ? c.MenuItem.Price.Value : 0 
            })
            .ToList();

            var model = new AddNewReservation
            {
                TableId = TableID,
                CustomerId = CustomerID,
                ReservationTime = DateTime.Now,
                CartItems = cartItems
            };

            LayDL();
            SetDefault(CustomerID, TableID);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddNewReservation reservation)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .Select(m => $"{m.Key}: {string.Join(", ", m.Value.Errors.Select(e => e.ErrorMessage))}");
                TempData["ErrorMessage"] = $"Dữ liệu đầu vào không hợp lệ: {string.Join("; ", errors)}";
                SetDefault(reservation.CustomerId, reservation.TableId);
                return View(reservation);
            }

            if (!_context.TblCustomers.Any(c => c.CustomerId == reservation.CustomerId))
            {
                TempData["ErrorMessage"] = $"Lỗi: Khách hàng với mã khách hàng {reservation.CustomerId} không tồn tại.";
                SetDefault(reservation.CustomerId, reservation.TableId);
                return View(reservation);
            }

            if (!_context.TblTables.Any(t => t.TableId == reservation.TableId))
            {
                TempData["ErrorMessage"] = $"Lỗi: Bàn với mã bàn {reservation.TableId} không tồn tại.";
                SetDefault(reservation.CustomerId, reservation.TableId);
                return View(reservation);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {   var branchId = HttpContext.Session.GetInt32("BranchID");
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

                if (newReservation.ReservationId == 0)
                {
                    TempData["ErrorMessage"] = "Lỗi: Không tạo được đơn đặt bàn.";
                    await transaction.RollbackAsync();
                    SetDefault(reservation.CustomerId, reservation.TableId);
                    return View(reservation);
                }

                if (reservation.CartItems == null || !reservation.CartItems.Any())
                {
                    TempData["ErrorMessage"] = $"Lỗi: Giỏ hàng trống cho mã khách hàng {reservation.CustomerId}. Vui lòng thêm món trước khi đặt bàn.";
                    await transaction.RollbackAsync();
                    SetDefault(reservation.CustomerId, reservation.TableId);
                    return View(reservation);
                }

                foreach (var item in reservation.CartItems)
                {
                    if (!_context.TblMenuItems.Any(m => m.MenuItemId == item.MenuItemId))
                    {
                        TempData["ErrorMessage"] = $"Lỗi: Món ăn với mã món ăn {item.MenuItemId} không tồn tại trong TblMenuItems.";
                        await transaction.RollbackAsync();
                        SetDefault(reservation.CustomerId, reservation.TableId);
                        return View(reservation);
                    }

                    if (item.Quantity <= 0)
                    {
                        TempData["ErrorMessage"] = $"Lỗi: Số lượng của món ăn với mã món ăn {item.MenuItemId} không hợp lệ.";
                        await transaction.RollbackAsync();
                        SetDefault(reservation.CustomerId, reservation.TableId);
                        return View(reservation);
                    }

                    _context.TblReservationMenuItems.Add(new TblReservationMenuItem
                    {
                        ReservationId = newReservation.ReservationId,
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity,
                        Note = item.Note
                    });
                    var menuitems =  await _context.TblMenuItems.FirstOrDefaultAsync(s => s.MenuItemId == item.MenuItemId);
                    if (menuitems is not null)
                    {
                        menuitems.Count_Saled += item.Quantity;
                        await _context.SaveChangesAsync();
                    }
                   
                }

                await _context.SaveChangesAsync();
                var dbCartItems = _context.TblCartItems
                    .Where(c => c.Cart.CustomerId == reservation.CustomerId)
                    .ToList();
                if (dbCartItems.Any())
                {
                    _context.TblCartItems.RemoveRange(dbCartItems);
                    await _context.SaveChangesAsync();
                }

                var table = await _context.TblTables.FirstOrDefaultAsync(s => s.TableId == reservation.TableId);
                    if (table == null)
                    {
                        TempData["ErrorMessage"] = $"Lỗi: Không tìm thấy bàn với mã bàn {reservation.TableId}.";
                        await transaction.RollbackAsync();
                        SetDefault(reservation.CustomerId, reservation.TableId);
                        return View(reservation);
                    }
                table.Status = "Đã được đặt";
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                TempData["SuccessMessage"] = JavaScriptEncoder.Default.Encode("Đặt bàn thành công!");
                return RedirectToAction("CreatePayment", "ReservationPayment", new { customerId = newReservation.CustomerId, reservationId = newReservation.ReservationId });
            }
            catch (DbUpdateException dbEx)
            {
                TempData["ErrorMessage"] = $"Lỗi cơ sở dữ liệu: {dbEx.Message}. Chi tiết: {dbEx.InnerException?.Message ?? "Không có chi tiết thêm."}";
                await transaction.RollbackAsync();
                SetDefault(reservation.CustomerId, reservation.TableId);
                return View(reservation);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi không xác định: {ex.Message}. Chi tiết: {ex.InnerException?.Message ?? "Không có chi tiết thêm."}";
                await transaction.RollbackAsync();
                SetDefault(reservation.CustomerId, reservation.TableId);
                return View(reservation);
            }
        } 
       public IActionResult Error()
        {
            return View("Error!");
        }
    }
} 