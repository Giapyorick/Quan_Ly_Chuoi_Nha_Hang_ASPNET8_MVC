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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;

        public ReservationController(QlChuoiNhaHang1Context context){
            _context = context;
        }
		public void LayDL()
		{
			var customer = _context.TblCustomers
			.Select(s => new SelectListItem
			{
				Value = s.CustomerId.ToString(),
				Text = s.FullName
			});
			var table = _context.TblTables
			.Select(s => new SelectListItem
			{
				Value = s.TableId.ToString(),
				Text = s.TableName
			});
			var branch = _context.TblBranches
			.Select(s => new SelectListItem
			{
				Value = s.BranchId.ToString(),
				Text = s.BranchName
			}
			);
			var menu = _context.TblMenuItems
			.Select(s => new SelectListItem
			{
				Value = s.MenuItemId.ToString(),
				Text = s.ItemName
			});
			ViewBag.Reservation_Customer = new SelectList(customer, "Value", "Text");
			ViewBag.Reservation_Table = new SelectList(table, "Value", "Text");
			ViewBag.Reservation_MenuItem = new SelectList(menu, "Value", "Text");
			ViewBag.Reservation_Branch = new SelectList(branch, "Value", "Text");
		}
		public IActionResult List()
		{
			var role = HttpContext.Session.GetString("Roles");
			var branch = HttpContext.Session.GetInt32("BranchID");

			if (string.IsNullOrEmpty(role) || branch == null)
			{
				return RedirectToAction("Login", "Login");
			}

			var query = _context.TblReservations
				.Include(s => s.TblReservationMenuItems)
				.AsQueryable();

			if (role is not "1")
			{
				query = query.Where(s => s.BranchId == branch);
			}

			var item = query.OrderByDescending(s => s.ReservationId).ToList();

			LayDL();
			return View(item);
		}

	}
}