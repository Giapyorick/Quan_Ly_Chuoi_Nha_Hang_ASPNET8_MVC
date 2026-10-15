using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using GiapDepTrai.Hubs;
using System.IO;
using System.Text;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Order/[action]")]
    public class OrderController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        private readonly IHubContext<KitchenHub> _hubContext;

        public OrderController(QlChuoiNhaHang1Context context, IHubContext<KitchenHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        private void LayDL()
        {
            var employee = _context.TblEmployees
                .Select(s => new SelectListItem
                {
                    Value = s.EmployeeId.ToString(),
                    Text = s.FullName
                }).ToList();

            var table = _context.TblTables
                .Select(s => new SelectListItem
                {
                    Value = s.TableId.ToString(),
                    Text = s.TableName
                }).ToList();

            var menuItem = _context.TblMenuItems
                .Select(s => new SelectListItem
                {
                    Value = s.MenuItemId.ToString(),
                    Text = s.ItemName
                }).ToList();

            var branch = _context.TblBranches
                .Select(s => new SelectListItem
                {
                    Value = s.BranchId.ToString(),
                    Text = s.BranchName
                }).ToList();

            ViewBag.Order_Employee = new SelectList(employee, "Value", "Text");
            ViewBag.Order_Table = new SelectList(table, "Value", "Text");
            ViewBag.Order_MenuItem = new SelectList(menuItem, "Value", "Text");
            ViewBag.Order_Branch = new SelectList(branch, "Value", "Text");
        }
        public IActionResult ChooseTable()
        {
            ViewBag.Order_EmployeeID_ChooseTB = HttpContext.Session.GetInt32("EmployeeID");
            ViewBag.Order_BranchID_ChooseTB = HttpContext.Session.GetInt32("BranchID");
            var branchId = HttpContext.Session.GetInt32("BranchID");
            var table = _context.TblTables.Include(m => m.TableType)
            .Where(s => s.BranchId == branchId)
            .ToList();
            if (TempData["AlertMessage"] != null)
            {
                ViewBag.AlertMessage = TempData["AlertMessage"];
            }
            return View(table);
        }
        public void SetDefault(int tableId, int employeeId, int branchId)
        {
            var employee = _context.TblEmployees.FirstOrDefault(s => s.EmployeeId == employeeId);
            if (employee != null)
            {
                ViewBag.EmployeeName = employee.FullName;
                ViewBag.EmployeeIMG = employee.Img;
            }

            var branch = _context.TblBranches.FirstOrDefault(s => s.BranchId == branchId);
            if (branch != null)
                ViewBag.BranchName = branch.BranchName;

            var table = _context.TblTables.FirstOrDefault(s => s.TableId == tableId);
            if (table != null)
                ViewBag.TableName = table.TableName;

            ViewBag.TableID = tableId;

            var menuItems = _context.TblMenuItems.ToList();
            ViewBag.Order_MenuItemAll = menuItems;
            ViewBag.Order_MenuItem = new SelectList(menuItems, "MenuItemId", "ItemName");
            
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
        public async Task<IActionResult> EmployeeOrder(int TableID, int EmployeeID, int BranchID)
        {
            if (!_context.TblTables.Any(t => t.TableId == TableID))
            {
                return NotFound("Không tìm thấy bàn.");
            }
            var promotion = await _context.TblPromotions
                .OrderByDescending(x => x.PromotionId)
                .FirstOrDefaultAsync();

            if (promotion != null)
            {
                ViewBag.Promotion = promotion.DiscountValue.ToString("0.##");
            }
            else
            {
                ViewBag.Promotion = "0";
            }

            ViewBag.TableID = TableID;
            SetDefault(TableID, EmployeeID, BranchID);

            if (TempData["AlertMessage"] != null)
            {
                ViewBag.AlertMessage = TempData["AlertMessage"];
            }

            var model = new TblEmployeeOrder
            {
                TableId = TableID,
                EmployeeId = EmployeeID,
                BranchId = BranchID
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeOrder(int TableID, int EmployeeID, int BranchID, TblMenuItem menuitem, List<TblEmployeeOrderDetail> details, List<TblInvoice> invoices)
        {
            if (!details.Any() || details.All(d => d.MenuItemId == 0 || d.Quantity <= 0))
            {
                ModelState.AddModelError("", "Vui lòng thêm ít nhất một món hợp lệ.");
                LayDL();
                ViewBag.TableID = TableID;
                return View(new TblEmployeeOrder { TableId = TableID, EmployeeId = EmployeeID, BranchId = BranchID });
            }

            if (!invoices.Any() || invoices.Any(i => i.TotalAmount < 0 || i.DiscountAmount < 0 || i.FinalAmount < 0 || i.FinalAmount != i.TotalAmount - i.DiscountAmount))
            {
                ModelState.AddModelError("", "Hóa đơn không hợp lệ. Vui lòng kiểm tra lại tổng tiền, chiết khấu và  cuối cùng.");
                LayDL();
                ViewBag.TableID = TableID;
                return View(new TblEmployeeOrder { TableId = TableID, EmployeeId = EmployeeID, BranchId = BranchID });
            }

            if (!_context.TblEmployees.Any(e => e.EmployeeId == EmployeeID) ||
                !_context.TblTables.Any(t => t.TableId == TableID) ||
                !_context.TblBranches.Any(b => b.BranchId == BranchID))
            {
                ModelState.AddModelError("", "Nhân viên, bàn hoặc chi nhánh không hợp lệ.");
                LayDL();
                ViewBag.TableID = TableID;
                ViewBag.EmployeeName = _context.TblEmployees.Find(EmployeeID)?.FullName;
                ViewBag.BranchName = _context.TblBranches.Find(BranchID)?.BranchName;
                ViewBag.TableName = _context.TblTables.Find(TableID)?.TableName;
                return View(new TblEmployeeOrder { TableId = TableID, EmployeeId = EmployeeID, BranchId = BranchID });
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newOrder = new TblEmployeeOrder
                {
                    TableId = TableID,
                    EmployeeId = EmployeeID,
                    BranchId = BranchID,
                    OrderTime = DateTime.Now
                };
                await _context.TblEmployeeOrders.AddAsync(newOrder);
                await _context.SaveChangesAsync();

                if (newOrder.OrderId <= 0)
                {
                    throw new Exception("Không thể tạo OrderID cho đơn hàng.");
                }

                var validDetails = details.Where(d => _context.TblMenuItems.Any(m => m.MenuItemId == d.MenuItemId) && d.Quantity > 0).ToList();
                if (!validDetails.Any())
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Không có chi tiết đơn hàng hợp lệ.");
                    LayDL();
                    ViewBag.TableID = TableID;
                    return View(new TblEmployeeOrder { TableId = TableID, EmployeeId = EmployeeID, BranchId = BranchID });
                }

                var orderDetailsMessage = validDetails.Select(d =>
                {
                    var menuItem = _context.TblMenuItems.Find(d.MenuItemId);
                    return $", {menuItem?.ItemName} (Số lượng: {d.Quantity} | Ghi chú:{d.Notes})";
                }).ToList();
                var notificationMessage = $"Đơn hàng mới (ID: {newOrder.OrderId}) từ bàn {TableID}:\n" +
                                         string.Join("\n", orderDetailsMessage);

                foreach (var detail in validDetails)
                {
                    var newDetail = new TblEmployeeOrderDetail
                    {
                        OrderId = newOrder.OrderId,
                        MenuItemId = detail.MenuItemId,
                        Quantity = detail.Quantity,
                        Notes = detail.Notes
                    };
                    await _context.TblEmployeeOrderDetails.AddAsync(newDetail);

                    var item = await _context.TblMenuItems.FindAsync(detail.MenuItemId);
                    if (item != null)
                    {
                        item.Count_Saled += detail.Quantity;
                    }
                }
                await _context.SaveChangesAsync();

                foreach (var invoice in invoices)
                {
                    var newInvoice = new TblInvoice
                    {
                        OrderId = newOrder.OrderId,
                        TotalAmount = invoice.TotalAmount,
                        DiscountAmount = invoice.DiscountAmount,
                        FinalAmount = invoice.FinalAmount,
                        Status = invoice.Status
                    };
                    await _context.TblInvoices.AddAsync(newInvoice);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                await _hubContext.Clients.Group("Kitchen").SendAsync("ReceiveOrderNotification", notificationMessage);
                SetDefault(TableID, EmployeeID, BranchID);
                TempData["AlertMessage"] = "Thêm đơn hàng và hóa đơn thành công!";
                return RedirectToAction("ChooseTable");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Lỗi khi lưu đơn hàng: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", $"Chi tiết lỗi: {ex.InnerException.Message}");
                }
                LayDL();
                ViewBag.EmployeeName = _context.TblEmployees.FirstOrDefault(s => s.EmployeeId == EmployeeID)?.FullName;
                ViewBag.BranchName = _context.TblBranches.FirstOrDefault(s => s.BranchId == BranchID)?.BranchName;
                ViewBag.TableName = _context.TblTables.FirstOrDefault(s => s.TableId == TableID)?.TableName;
                return View(new TblEmployeeOrder { TableId = TableID, EmployeeId = EmployeeID, BranchId = BranchID });
            }
        }   
        [HttpPost]
        public async Task<IActionResult> GenerateReceipt(int orderId)
        {
            if (orderId <= 0)
            {
                TempData["ErrorMessage"] = "ID đơn hàng không hợp lệ.";
                return RedirectToAction("ChooseTable");
            }

            var order = await _context.TblEmployeeOrders
                .Include(o => o.TblEmployeeOrderDetails)
                .ThenInclude(d => d.MenuItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null || !order.TblEmployeeOrderDetails.Any())
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng hoặc chi tiết đơn hàng.";
                return RedirectToAction("ChooseTable");
            }

            var receiptContent = GenerateReceiptContent(order, order.TblEmployeeOrderDetails.ToList());
            var fileName = $"Order_{order.OrderId}_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "receipts", fileName);

            var receiptsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "receipts");
            if (!Directory.Exists(receiptsDir))
            {
                Directory.CreateDirectory(receiptsDir);
            }
            await System.IO.File.WriteAllTextAsync(filePath, receiptContent, Encoding.UTF8);

            var invoice = await _context.TblInvoices.FirstOrDefaultAsync(x => x.OrderId == orderId);
            if (invoice != null)
            {
                invoice.Status = "Đã thanh toán";
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật thất bại";
            }


            TempData["SuccessMessage"] = "In hóa đơn thành công";
            return RedirectToAction("List","Invoice");
        }
        private string GenerateReceiptContent(TblEmployeeOrder order, List<TblEmployeeOrderDetail> details)
        {
            var sb = new StringBuilder();
            sb.AppendLine("===== HÓA ĐƠN GỌI MÓN =====");
            sb.AppendLine($"ID Đơn hàng: {order.OrderId}");
            sb.AppendLine($"Bàn: {order.TableId}");
            sb.AppendLine($"Thời gian: {order.OrderTime:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"Chi nhánh: {order.BranchId}");
            sb.AppendLine($"Nhân viên: {order.EmployeeId}");
            sb.AppendLine("----------------------------");
            sb.AppendLine("Chi tiết món:");

            var menuItemIds = details.Select(d => d.MenuItemId).Distinct().ToList();
            var menuItems = _context.TblMenuItems
                .Where(m => menuItemIds.Contains(m.MenuItemId))
                .ToDictionary(m => m.MenuItemId, m => m);

            decimal totalAmount = 0;
            foreach (var detail in details)
            {
                var menuItem = menuItems.ContainsKey(detail.MenuItemId) ? menuItems[detail.MenuItemId] : null;
                var price = (menuItem?.Price ?? 0) * detail.Quantity;
                totalAmount += price;
                sb.AppendLine($"- {menuItem?.ItemName ?? "Unknown"} \n({detail.Quantity} : {price:C}" +
                            (!string.IsNullOrEmpty(detail.Notes) ? $" : {detail.Notes})" : ""));
            }
            sb.AppendLine("----------------------------");
            sb.AppendLine($"Tổng tiền: {totalAmount:C}");
            sb.AppendLine("Cảm ơn quý khách!");
            sb.AppendLine("=============================");

            return sb.ToString();
        }  
        [HttpGet]
        public IActionResult Kitchen(int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var model = new KitchenModel
            {
                RecentOrders = _context.TblEmployeeOrders
                    .Include(o => o.TblEmployeeOrderDetails)
                    .ThenInclude(d => d.MenuItem)
                    .OrderByDescending(o => o.OrderTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()
            };
            int totalItems = _context.TblEmployeeOrders.Count();
            ViewBag.TotalPagesKitchen = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageKitchen = page;
            return View(model);
        }
        public IActionResult List()
        {
            return Content("Order created successfully!");
        }
    }
}