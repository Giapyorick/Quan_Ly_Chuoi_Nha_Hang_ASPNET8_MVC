using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;

        public HomeController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employeeID = HttpContext.Session.GetInt32("EmployeeID");

            if (!employeeID.HasValue)
            {
                TempData["ErrorMessage"] = "Phiên đăng nhập đã hết. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Login");
            }

            var item = _context.TblEmployees
            .Include(s => s.Branch)
            .FirstOrDefault(s => s.EmployeeId == employeeID.Value);

            if (item == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin nhân viên.";
                return RedirectToAction("Login", "Login");
            }
                HttpContext.Session.SetString("Home_EmployeeName",item.FullName);
            if(item.Branch is not null){
                HttpContext.Session.SetString("Home_BranchName", item.Branch.BranchName);
            }
            else{
                TempData["ErrorMessage"] = "Lỗi Item.Branch";
            }
            if(item.Img is not null){
                HttpContext.Session.SetString("Home_EmployeeIMG",item.Img);
            }
            else{
                TempData["ErrorMessage"] = "Không tồn tại địa chỉ ảnh";
            }
            return View();
        }

        [HttpGet]
        public IActionResult Test()
        {
            return View();
        }
    }
}
