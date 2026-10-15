using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;

        public LoginController(QlChuoiNhaHang1Context context){
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(TblAccount user)
        {
            if (user is null)
            {
                return NotFound();
            }

            string pw = Functions.MD5Password(user.PassWords);
            var check = _context.TblAccounts
                .Where(a => a.UserName == user.UserName && a.PassWords == pw)
                .FirstOrDefault();

            if (check is null)
            {
                TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu sai!";
                return RedirectToAction("Login", "Login");
            }
            if (check.EmployeeId.HasValue)
            {
                HttpContext.Session.SetInt32("EmployeeID", check.EmployeeId.Value);
                var employee = _context.TblEmployees
                    .FirstOrDefault(e => e.EmployeeId == check.EmployeeId.Value);

                if (employee != null && employee.BranchId.HasValue)
                {
                    HttpContext.Session.SetInt32("BranchID", employee.BranchId.Value);
                }
            }

            HttpContext.Session.SetString("UserName", check.UserName ?? string.Empty);
            HttpContext.Session.SetString("Roles", check.Roles ?? string.Empty);

            return RedirectToAction("Index", "Home");
        }

    }
}