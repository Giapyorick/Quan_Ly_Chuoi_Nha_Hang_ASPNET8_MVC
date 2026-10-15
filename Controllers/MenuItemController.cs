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
    public class MenuItemController : Controller
    {
        private QlChuoiNhaHang1Context _context;
        public MenuItemController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var menuItem = await _context.TblMenuItems
                .Include(m => m.Category)
                .Include(m => m.Branch)
                .Include(m => m.TblComments)
                    .ThenInclude(c => c.Customer)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}