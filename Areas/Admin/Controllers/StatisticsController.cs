using System;
using System.Collections.Generic;
using System.Linq;
using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticsController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;

        public StatisticsController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public void LayDL(int n)
        {
            int selectedBranchId = n;
            var branches = _context.TblBranches
                .Select(t => new
                {
                    Value = t.BranchId,
                    Text = t.BranchName
                }).ToList();

            ViewBag.Branches = branches;
            ViewBag.Branch = selectedBranchId;

            var item = _context.TblBranches.FirstOrDefault(x => x.BranchId == n);
            if (item is not null)
            {
                ViewBag.BranchName = item.BranchName;
            }

        }
        private List<MenuItemStatisticViewModel> GetTopSellingItems(int n, int m)
        {
            var role = HttpContext.Session.GetString("Roles");
            List<MenuItemStatisticViewModel> data;

            if (role == "1")
            {
                data = _context.TblReservations
                .Where(r => r.BranchId == m)
                .Include(r => r.TblReservationMenuItems)
                    .ThenInclude(ri => ri.MenuItem)
                .SelectMany(r => r.TblReservationMenuItems.Select(ri => new
                {
                    r.BranchId,
                    ri.MenuItem.ItemName,
                    ri.Quantity
                }))
                .GroupBy(x => new { x.BranchId, x.ItemName })
                .Select(g => new MenuItemStatisticViewModel
                {
                    ItemName = g.Key.ItemName,
                    CountSaled = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.CountSaled)
                .Take(n)
                .ToList();
            }
            else if (role == "2")
            {
                var branchId = HttpContext.Session.GetInt32("BranchID");
                if (branchId == null)
                    return new List<MenuItemStatisticViewModel>();

                data = _context.TblReservations
                .Where(r => r.BranchId == branchId)
                .Include(r => r.TblReservationMenuItems)
                    .ThenInclude(ri => ri.MenuItem)
                .SelectMany(r => r.TblReservationMenuItems.Select(ri => new
                {
                    r.BranchId,
                    ri.MenuItem.ItemName,
                    ri.Quantity
                }))
                .GroupBy(x => new { x.BranchId, x.ItemName })
                .Select(g => new MenuItemStatisticViewModel
                {
                    ItemName = g.Key.ItemName,
                    CountSaled = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.CountSaled)
                .Take(n)
                .ToList();
            }
            else
            {
                return new List<MenuItemStatisticViewModel>();
            }

            return data ;
        }

        public IActionResult TopSelling(int n, int m)
        {
            var data = GetTopSellingItems(n, m);
            return View(data); 
        }

        public async Task<IActionResult> TopSellingMenuItems(int top = 10, int branch = 1)
        {
            ViewBag.Top = top;
            ViewBag.BranchTop = branch;

            var branchId = HttpContext.Session.GetInt32("BranchID");
            ViewBag.Emp_BranchID = branchId;

            if (branchId != null)
            {
                var branches = await _context.TblBranches.FirstOrDefaultAsync(s => s.BranchId == branchId);
                ViewBag.Emp_BranchName = branches?.BranchName ?? "Chi nhánh không tồn tại";
            }
            else
            {
                ViewBag.Emp_BranchName = "Không tìm thấy BranchID trong Session";
            }

            var data = GetTopSellingItems(top, branch); 
            LayDL(branch);
            return View(data); 
        }

        public List<InvoiceStatisticsViewModel> Statistics(int n, int m)
        {
            DateTime today = DateTime.Today;
            DateTime start = today.AddDays(n);

            var data = _context.TblEmployeeOrders
                .Include(x => x.TblInvoices)
                .Where(x => x.OrderTime != null &&
                            x.OrderTime >= start &&
                            x.OrderTime < today.AddDays(1) &&
                            x.BranchId == m
                            )
                .GroupBy(x => x.OrderTime.Value.Date)
                .OrderBy(x => x.Key)
                .Select(g => new InvoiceStatisticsViewModel
                {
                    Date = g.Key.ToString("dd/MM/yyyy"),
                    TotalAmount = g.SelectMany(o => o.TblInvoices).Sum(i => i.FinalAmount)
                })
                .ToList();

            return data;
        }

        public async Task<IActionResult> TotalAmount(int day = -7, int branch = 1)
        {
            ViewBag.Day = day;
            ViewBag.Branch = branch;
            var branchId = HttpContext.Session.GetInt32("BranchID");
            ViewBag.Emp_BranchID = branchId;

            if (branchId != null)
            {
                var branches = await _context.TblBranches.FirstOrDefaultAsync(s => s.BranchId == branchId);
                ViewBag.Emp_BranchName = branches?.BranchName ?? "Chi nhánh không tồn tại";
            }
            else
            {
                ViewBag.Emp_BranchName = "Không tìm thấy BranchID trong Session";
            }
            LayDL(branch);
            var data = Statistics(day, branch);
            return View(data);
        }

        private List<ComparedToBranchesViewModel> Compared(int n)
        {
            DateTime today = DateTime.Today;
            DateTime start = today.AddDays(n);
            var data = _context.TblEmployeeOrders
                .Include(b => b.Branch)
                .Include(o => o.TblEmployeeOrderDetails)
                    .ThenInclude(d => d.MenuItem)
                .Where(x => x.OrderTime != null &&
                            x.OrderTime >= start &&
                            x.OrderTime < today.AddDays(1)
                            )
                .GroupBy(o => o.BranchId)
                .Select(g => new ComparedToBranchesViewModel
                {
                    BranchName = g.First().Branch.BranchName,
                    TotalAmount = g.SelectMany(o => o.TblEmployeeOrderDetails)
                                    .Sum(d => d.Quantity * d.MenuItem.Price)
                })
                .ToList();
            return data;
        }
        public IActionResult ComparedToBranches( int day = -7)
        {
            ViewBag.Day = day;
            var data = Compared(day);
            return View(data);
        }

    }
}