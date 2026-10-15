using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GiapDepTrai.Controllers
{
    public class ProfileController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public ProfileController(QlChuoiNhaHang1Context context) {
            _context = context;
        }
        public IActionResult Profile()
        {
            var CustomerId = HttpContext.Session.GetInt32("CustomerID");
            var profile = _context.TblCustomers
            .FirstOrDefault(s => s.CustomerId == CustomerId);
            return View(profile);
        }
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}