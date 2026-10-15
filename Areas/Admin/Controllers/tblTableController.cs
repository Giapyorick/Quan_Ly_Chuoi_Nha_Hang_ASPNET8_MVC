using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblTableController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public tblTableController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public void LayDL()
        {
            var branch = _context.TblBranches
            .Select(s => new SelectListItem
            {
                Value = s.BranchId.ToString(),
                Text = s.BranchName
            });
            var type = _context.TblTableTypes
            .Select(s => new SelectListItem
            {
                Value = s.TableTypeId.ToString(),
                Text = s.TypeName
            });
            ViewBag.Table_Branch = new SelectList(branch, "Value", "Text");
            ViewBag.Table_TypeTable = new SelectList(type, "Value", "Text");

        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var branchId = HttpContext.Session.GetInt32("BranchID");
            ViewBag.Emp_BranchID = branchId;

            if (branchId != null)
            {
                var branch = await _context.TblBranches.FirstOrDefaultAsync(s => s.BranchId == branchId);
                if (branch != null)
                {
                    ViewBag.Emp_BranchName = branch.BranchName;
                }
                else
                {
                    ViewBag.Emp_BranchName = "Chi nhánh không tồn tại";
                }
            }
            else
            {
                ViewBag.Emp_BranchName = "Không tìm thấy BranchID trong Session";
            }
            LayDL();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddNewTable table)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                }
                return View(table);
            }

            var item = new TblTable
            {
                TableName = table.TableName,
                TableTypeId = table.TableTypeId,
                BranchId = table.BranchId,
                Status = table.Status
                
            };

            await _context.TblTables.AddAsync(item);
            await _context.SaveChangesAsync();
            LayDL();
            return RedirectToAction("List", new { page = 1 });
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchTable, int page = 1, int pageSize = 5)
        {
            var query = _context.TblTables.AsQueryable();
            /*ViewBag.Rolec = HttpContext.Session.GetString("Role");*/
            if (!string.IsNullOrWhiteSpace(searchTable))
            {
                query = query.Where(s =>
                    (!string.IsNullOrEmpty(s.TableName) && s.TableName.Contains(searchTable)) ||
                    (s.BranchId.HasValue && s.BranchId.Value.ToString().Contains(searchTable)) ||
                    (s.TableTypeId.HasValue && s.TableTypeId.Value.ToString().Contains(searchTable)) ||
                    (!string.IsNullOrEmpty(s.Status)&& s.Status.Contains(searchTable))
                );
            }
            int totalItems = await query.CountAsync();
            var branch = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesTable = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageTable = page;
            LayDL();
            return View(branch);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var branchId = HttpContext.Session.GetInt32("BranchID");
            ViewBag.Emp_BranchID = branchId;

            if (branchId != null)
            {
                var branch = await _context.TblBranches.FirstOrDefaultAsync(s => s.BranchId == branchId);
                if (branch != null)
                {
                    ViewBag.Emp_BranchName = branch.BranchName;
                }
                else
                {
                    ViewBag.Emp_BranchName = "Chi nhánh không tồn tại";
                }
            }
            else
            {
                ViewBag.Emp_BranchName = "Không tìm thấy BranchID trong Session";
            }
            var table = await _context.TblTables.FindAsync(id);
            LayDL();
            return View(table);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblTable table)
        {
            var item = await _context.TblTables.FindAsync(table.TableId);
            if (item is not null)
            {
                item.TableName = table.TableName;
                item.TableTypeId = table.TableTypeId;
                item.BranchId = table.BranchId;
                item.Status = table.Status;
                await _context.SaveChangesAsync();
            }
            LayDL();
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblTable table)
        {
            try
            {
                var item = await _context.TblTables.AsNoTracking().FirstOrDefaultAsync(b => b.TableId == table.TableId);
                if (item is not null)
                {
                    _context.TblTables.Remove(item);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa thành công!";
                }
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Không thể xóa. Dữ liệu đang được sử dụng.";

            }
            return RedirectToAction("List");
        }
    }
}
