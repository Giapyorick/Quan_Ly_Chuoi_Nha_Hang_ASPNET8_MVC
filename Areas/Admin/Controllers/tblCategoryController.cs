using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblCategoryController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public tblCategoryController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddNewCategory category)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["ErrorMessage"] = "ModelState Error: " + error.ErrorMessage;
                }
                return View(category);
            }

            var item = new TblCategory
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive,
            };

            await _context.TblCategories.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("List", new { page = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchCategory, int page = 1, int pageSize = 5)
        {
            var query = _context.TblCategories.AsQueryable();
            /*ViewBag.Rolec = HttpContext.Session.GetString("Role");*/
            if (!string.IsNullOrWhiteSpace(searchCategory))
            {
                query = query.Where(s =>
                    (!string.IsNullOrEmpty(s.CategoryName) && s.CategoryName.Contains(searchCategory)) 
                );
            }
            int totalItems = await query.CountAsync();
            var cate = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesCategory = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageCategory = page;

            return View(cate);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.TblCategories.FindAsync(id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblCategory category)
        {
            var item = await _context.TblCategories.FindAsync(category.CategoryId);
            if (item is not null)
            {
                item.CategoryId = category.CategoryId;
                item.CategoryName = category.CategoryName;
                item.IsActive = category.IsActive;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblCategory category)
        {
            try
            {
                var item = await _context.TblCategories.AsNoTracking().FirstOrDefaultAsync(b => b.CategoryId == category.CategoryId);
                if (item is not null)
                {
                    _context.TblCategories.Remove(item);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa thành công!";
                }
            }
            catch (DbUpdateException )
            {
                TempData["ErrorMessage"] = "Không thể xóa. Dữ liệu đang được sử dụng.";

            }
            return RedirectToAction("List");
        }
    }
}
