using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderDetailController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public OrderDetailController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public void LayDL()
        {
            var employee = _context.TblEmployees
                .Select(s => new SelectListItem
                {
                    Value = s.EmployeeId.ToString(),
                    Text = s.FullName
                });
            var table = _context.TblTables
                .Select(s => new SelectListItem
                {
                    Value = s.TableId.ToString(),
                    Text = s.TableName
                });
            var menuItem = _context.TblMenuItems
                .Select(s => new SelectListItem
                {
                    Value = s.MenuItemId.ToString(),
                    Text = s.ItemName

                });
            var branch = _context.TblBranches
                .Select(s => new SelectListItem
                {
                    Value = s.BranchId.ToString(),
                    Text = s.BranchName
                });
            ViewBag.OrderDetail_Employee = new SelectList(employee, "Value", "Text");
            ViewBag.OrderDetail_Table = new SelectList(table, "Value", "Text");
            ViewBag.OrderDetail_MenuItem = new SelectList(menuItem, "Value", "Text");
            ViewBag.OrderDetail_Branch = new SelectList(branch, "Value", "Text");

        }
        [HttpGet]
        public IActionResult List(TblEmployeeOrderDetail detail)
        {
            var list = _context.TblEmployeeOrders
                .Include(s => s.TblEmployeeOrderDetails)
                .OrderByDescending(s => s.OrderId)
                .ToList();
                
            LayDL();
            return View(list);
        }
    }
}
