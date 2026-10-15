using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoiceController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public InvoiceController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public void LayDL()
        {
            var menuItem = _context.TblMenuItems
                .Select(s => new SelectListItem
                {
                    Value = s.MenuItemId.ToString(),
                    Text = s.ItemName
                });
            var employee = _context.TblEmployees
                .Select(s => new SelectListItem
                {
                    Value = s.EmployeeId.ToString(),
                    Text = s.FullName
                });
            var table = _context.TblTables
                .Select(s => new SelectListItem{
                    Value = s.TableId.ToString(),
                    Text = s.TableName
                });
            var branch = _context.TblBranches
                .Select(s => new SelectListItem
                {
                    Value = s.BranchId.ToString(),
                    Text = s.BranchName
                });

            ViewBag.Invoice_MenuItem = new SelectList(menuItem, "Value", "Text");
            ViewBag.Invoice_Employee = new SelectList(employee, "Value", "Text");
            ViewBag.Invoice_Branch = new SelectList(branch, "Value", "Text");
            ViewBag.Invoice_Table = new SelectList(table,"Value","Text");
        }
        public IActionResult List()
        {
            var item = _context.TblEmployeeOrders
                .Include(i => i.TblEmployeeOrderDetails)
                .Include(i => i.TblInvoices)
                .OrderByDescending(i => i.OrderId)
                .ToList();
            LayDL();
            return View(item);
        }
        public IActionResult Payment()
        {
            var item = _context.TblInvoices
                .Include(i => i.TblPayments)
                .ToList();
            LayDL();
            return View(item);
        }
    }
}
