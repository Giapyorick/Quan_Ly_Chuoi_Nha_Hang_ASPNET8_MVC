using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblTableTypeController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public tblTableTypeController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddNewTableType table)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                }
                return View(table);
            }

            var item = new TblTableType
            {
                TableTypeId = table.TableTypeId,
                TypeName = table.TypeName,
                Seats = table.Seats,
            };

            await _context.TblTableTypes.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("List", new { page = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchTableType, int page = 1, int pageSize = 5)
        {
            var query = _context.TblTableTypes.AsQueryable();
            /*ViewBag.Rolec = HttpContext.Session.GetString("Role");*/
            if (!string.IsNullOrWhiteSpace(searchTableType))
            {
                query = query.Where(s =>
                    (!string.IsNullOrEmpty(s.TypeName) && s.TypeName.Contains(searchTableType)) ||
                    (s.Seats.HasValue && s.Seats.Value.ToString().Contains(searchTableType))
                );
            }
            int totalItems = await query.CountAsync();
            var tableTypes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesTableType = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageTableType = page;

            return View(tableTypes);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.TblTableTypes.FindAsync(id);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblTableType tableType)
        {
            var item = await _context.TblTableTypes.FindAsync(tableType.TableTypeId);
            if (item is not null)
            {
                item.TableTypeId = tableType.TableTypeId;
                item.TypeName = tableType.TypeName;
                item.Seats = tableType.Seats;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblTableType tableType)
        {
            try
            {
                var item = await _context.TblTableTypes.AsNoTracking().FirstOrDefaultAsync(b => b.TableTypeId == tableType.TableTypeId);
                if (item is not null)
                {
                    _context.TblTableTypes.Remove(item);
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
