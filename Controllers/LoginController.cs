using System;
using System.Linq;
using System.Threading.Tasks;
using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace GiapDepTrai.Controllers
{
    public class LoginController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;

        public LoginController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(TblCustomer user, string? returnUrl)
        {
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6Ld0qUgrAAAAAGwFj_Y1dWfvjuQbgGYbTZhy1XnP";

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}",
                null);

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            if (result.success != true)
            {
                TempData["ErrorMessage"] = "Vui lòng xác thực reCAPTCHA.";
                return View(user);
            }
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Passwork))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập email và mật khẩu.";
                return View(user);
            }

            string pw = Functions.MD5Password(user.Passwork);
            var check = _context.TblCustomers
                .FirstOrDefault(a => a.Email == user.Email && a.Passwork == pw);

            if (check == null)
            {
                TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu sai!";
                return View(user);
            }

            HttpContext.Session.SetInt32("CustomerID", check.CustomerId.Value);
            HttpContext.Session.SetString("CustomerName", check.FullName ?? string.Empty);
            HttpContext.Session.SetString("Email", check.Email ?? string.Empty);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            Console.WriteLine("ReturnUrl không hợp lệ hoặc null.");
        }


            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AddNewCustommer custommer)
        {
            if (string.IsNullOrEmpty(custommer.Email) || string.IsNullOrEmpty(custommer.Passwork))
            {
                TempData["ErrorMessage"] = "Email hoặc mật khẩu không được để trống.";
                return View(custommer);
            }

            if (ModelState.IsValid)
            {
                if (await _context.TblCustomers.AnyAsync(c => c.Email == custommer.Email))
                {
                    TempData["ErrorMessage"] = "Email đã được sử dụng. Vui lòng chọn email khác.";
                    return View(custommer);
                }

                var item = new TblCustomer
                {
                    FullName = custommer.FullName,
                    Gender = custommer.Gender,
                    Address = custommer.Address,
                    Phone = custommer.Phone,
                    Email = custommer.Email,
                    Passwork = Functions.MD5Password(custommer.Passwork),
                    CreatedAt = DateTime.Now,
                    IdentityCard = custommer.IdentityCard,
                    BirthofDate = custommer.BirthofDate,
                    Img = custommer.Img,
                    Status = custommer.Status ?? true
                };

                _context.TblCustomers.Add(item);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Login");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["ErrorMessage"] = string.Join("; ", errors);
            return View(custommer);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}