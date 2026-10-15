using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblPromotionController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;

        public tblPromotionController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddNewPromotions promo)
        {
            if (!ModelState.IsValid)
            {
                return View(promo);
            }
            var item = new TblPromotion
            {
                PromotionName = promo.PromotionName,
                Description = promo.Description,
                DiscountType = promo.DiscountType,
                DiscountValue = promo.DiscountValue,
                StartDate = promo.StartDate,
                EndDate = promo.EndDate,
                IsActive = promo.IsActive,
                MinOrderAmount = promo.MinOrderAmount
            };
            await _context.TblPromotions.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("List", new { page = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchPromotion, int page = 1, int pageSize = 5)
        {
            var query = _context.TblPromotions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchPromotion))
            {
                query = query.Where(p =>
                    (!string.IsNullOrEmpty(p.PromotionName) && p.PromotionName.Contains(searchPromotion)) ||
                    (!string.IsNullOrEmpty(p.DiscountType) && p.DiscountType.Contains(searchPromotion)) ||
                    (p.MinOrderAmount.HasValue && p.MinOrderAmount.Value.ToString().Contains(searchPromotion))
                );
            }

            int totalItems = await query.CountAsync();
            var promotions = await query
                .OrderByDescending(p => p.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            return View(promotions);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var promo = await _context.TblPromotions.FindAsync(id);
            if (promo == null) return NotFound();
            return View(promo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TblPromotion promo)
        {
            var item = await _context.TblPromotions.FindAsync(promo.PromotionId);
            if (item != null)
            {
                item.PromotionName = promo.PromotionName;
                item.Description = promo.Description;
                item.DiscountType = promo.DiscountType;
                item.DiscountValue = promo.DiscountValue;
                item.StartDate = promo.StartDate;
                item.EndDate = promo.EndDate;
                item.IsActive = promo.IsActive;
                item.MinOrderAmount = promo.MinOrderAmount;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("List", new { page = 1 });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TblPromotion promo)
        {
            try
            {
                var item = await _context.TblPromotions.AsNoTracking().FirstOrDefaultAsync(p => p.PromotionId == promo.PromotionId);
                if (item != null)
                {
                    _context.TblPromotions.Remove(item);
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
