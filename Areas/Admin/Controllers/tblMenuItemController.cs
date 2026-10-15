using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblMenuItemController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        private readonly IWebHostEnvironment _iwebHostEnvironment;

        public tblMenuItemController(QlChuoiNhaHang1Context context, IWebHostEnvironment iwebHostEnvironment)
        {
            _context = context;
            _iwebHostEnvironment = iwebHostEnvironment;
        }
        [HttpGet]
        public IActionResult Add()
        {
            LayDL();
            return View();
        }
        public void LayDL()
        {
            var employee = _context.TblBranches
                .Select(t => new SelectListItem
                {
                    Value = t.BranchId.ToString(),
                    Text = t.BranchName
                }).ToList();
            var category = _context.TblCategories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.MenuItem_Branch = new SelectList(employee, "Value", "Text");
            ViewBag.MenuItem_Category = new SelectList(category, "Value", "Text");
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddNewMenuItem menu)
        {

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["ErrorMessage"] = error.ErrorMessage;
                }
                return View(menu);
            }
            if (menu.formFile != null && menu.formFile.Length > 0)
            {
                string Diachitep = Path.Combine(_iwebHostEnvironment.WebRootPath, "assets", "images", "MenuItem");
                if (!Directory.Exists(Diachitep))
                {
                    Directory.CreateDirectory(Diachitep);
                }
                string tenAnh = $"MenuItem_{Guid.NewGuid()}.jpg";
                string diachiAnh = Path.Combine(Diachitep, tenAnh);
                using (var stream = new FileStream(diachiAnh, FileMode.Create))
                {
                    await menu.formFile.CopyToAsync(stream);
                }
                menu.ImageUrl = $"assets/images/MenuItem/{tenAnh}";
            }
            var item = new TblMenuItem
            { 
                ItemName = menu.ItemName,
                Descriptions = menu.Descriptions,
                Price = menu.Price,
                CategoryId = menu.CategoryId,
                ImageUrl = menu.ImageUrl,
                IsAvailable = menu.IsAvailable,
                BranchId = 1,
                Count_Saled = 0
            };
            await _context.TblMenuItems.AddAsync(item);
            await _context.SaveChangesAsync();
            TempData["ErrorMessage"] = "Thêm thông tin thực đơn thành công!";
            LayDL();
            return RedirectToAction("List", new { page = 1 });
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchMenuItem, int page = 1, int pageSize = 5)
        {
            LayDL();
            var query = _context.TblMenuItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchMenuItem))
            {
                bool isNumeric = decimal.TryParse(searchMenuItem, out decimal searchNumber);

                query = query.Where(s =>
                    (!string.IsNullOrEmpty(s.ItemName) && s.ItemName.Contains(searchMenuItem))
                );
            }
            int totalItems = await query.CountAsync();
            var item = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesMenuItem = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageMenuItem = page;

            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult>Edit(int id)
        {
            var item = await _context.TblMenuItems.FindAsync(id);
            LayDL();
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult>Edit(TblMenuItem menu, IFormFile? formFile)
        {

            var item = await _context.TblMenuItems.FindAsync(menu.MenuItemId);
            if (item is null)
            {
                return NotFound();
            }
            else
            {
                item.MenuItemId = menu.MenuItemId;
                item.ItemName = menu.ItemName;
                item.Descriptions = menu.Descriptions;
                item.CategoryId = menu.CategoryId;
                item.Price = menu.Price;
                item.ImageUrl = menu.ImageUrl;
                item.IsAvailable = menu.IsAvailable;
                item.BranchId = menu.BranchId;
                await _context.SaveChangesAsync();

                if (formFile != null && formFile.Length > 0)
                {
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    string oldImagePath = Path.Combine(_iwebHostEnvironment.WebRootPath, item.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string diachitep = Path.Combine(_iwebHostEnvironment.WebRootPath, "assets", "images", "MenuItem");
                if (!Directory.Exists(diachitep))
                {
                    Directory.CreateDirectory(diachitep);
                }
                string tenAnh = $"MenuItem_{Guid.NewGuid()}.jpg";
                string diachiAnh = Path.Combine(diachitep, tenAnh);
                using (var stream = new FileStream(diachiAnh, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                item.ImageUrl = $"assets/images/MenuItem/{tenAnh}";
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Chỉnh sửa thông tin thực đơn thành công";
            }
            LayDL();
            return RedirectToAction("List", new { page = 1 });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblMenuItem menu)
        {
            try
            {
                var item = await _context.TblMenuItems.AsNoTracking().FirstOrDefaultAsync(s => s.MenuItemId == menu.MenuItemId);
                if (item is not null)
                {
                    if (!string.IsNullOrEmpty(menu.ImageUrl))
                    {
                        string diachiAnh = Path.Combine(_iwebHostEnvironment.WebRootPath, menu.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(diachiAnh))
                        {
                            System.IO.File.Delete(diachiAnh);
                        }
                    }
                    _context.TblMenuItems.Remove(item);
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
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                List<TblMenuItem> list;
                using (var context = new QlChuoiNhaHang1Context())
                {
                    list = await context.TblMenuItems.ToListAsync();
                }

                var curentdate = DateTime.Now.ToString("yyyyMMdd_HHmmss");


                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("tblMenuItem");

                    worksheet.Cells["A1"].Value = "Mã món";
                    worksheet.Cells["B1"].Value = "Tên món";
                    worksheet.Cells["C1"].Value = "Mô tả";
                    worksheet.Cells["D1"].Value = "Gía";
                    worksheet.Cells["E1"].Value = "Mã danh mục";
                    worksheet.Cells["F1"].Value = "Ảnh";
                    worksheet.Cells["G1"].Value = "Trạng thái";
                    worksheet.Cells["H1"].Value = "Mã chi nhánh";

                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = list[i].MenuItemId;
                        worksheet.Cells[i + 2, 2].Value = list[i].ItemName;
                        worksheet.Cells[i + 2, 3].Value = list[i].Descriptions;
                        worksheet.Cells[i + 2, 4].Value = list[i].Price;
                        worksheet.Cells[i + 2, 5].Value = list[i].CategoryId;
                        worksheet.Cells[i + 2, 6].Value = list[i].ImageUrl;
                        worksheet.Cells[i + 2, 7].Value = list[i].IsAvailable;
                        worksheet.Cells[i + 2, 8].Value = list[i].BranchId;

                    }

                    worksheet.Cells.AutoFitColumns();

                    var stream = new MemoryStream(await package.GetAsByteArrayAsync());
                    stream.Position = 0;
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"TblMenuItem{curentdate}.xlsx");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }

        }
        [HttpGet]
        public IActionResult ViewImport()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn một tệp Excel.";
                return RedirectToAction("ViewImport");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;


                        using (var context = new QlChuoiNhaHang1Context())
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                var ItemName = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                                var descriptions = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                                decimal? price = null;
                                var value = worksheet.Cells[row, 3].Value;
                                if (value != null && decimal.TryParse(value.ToString(), out decimal parsedValue))
                                {
                                    price = parsedValue;
                                }
                                int? cate = null;
                                var CategoryId = worksheet.Cells[row, 4].Value;
                                if (CategoryId != null && int.TryParse(CategoryId.ToString(), out int parsedCate))
                                {
                                    cate = parsedCate;
                                }
                                var IsAvailable = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                                int? branch = null;
                                var BranchId = worksheet.Cells[row, 6].Value;
                                if (BranchId != null && int.TryParse(BranchId.ToString(), out int parsedBranch))
                                {
                                    branch = parsedBranch;
                                }
                                if (string.IsNullOrEmpty(ItemName))
                                {
                                    TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Thiếu tên khách hàng (dữ liệu bắt buộc).";
                                    continue; 
                                }
                                bool? statusValue = null;
                                if (!string.IsNullOrEmpty(IsAvailable))
                                {
                                    if (IsAvailable.Equals("true", StringComparison.OrdinalIgnoreCase))
                                    {
                                        statusValue = true;
                                    }
                                    else if (IsAvailable.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                                             IsAvailable.Equals("0") ||
                                             IsAvailable.Equals("Không hoạt động", StringComparison.OrdinalIgnoreCase))
                                    {
                                        statusValue = false;
                                    }
                                    else
                                    {
                                        TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Giá trị 'IsAvailable' không hợp lệ ({IsAvailable}).";
                                        continue; 
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Thiếu giá trị 'IsAvailable'.";
                                    continue; 
                                }
                                var tblMenuitem = new TblMenuItem
                                {
                                    ItemName = ItemName,
                                    Descriptions = descriptions ?? "null",
                                    Price = price,
                                    CategoryId = cate,
                                    IsAvailable = statusValue.Value,
                                    BranchId = branch,
                                    Count_Saled = 0
 
                                };

                                context.TblMenuItems.Add(tblMenuitem);
                            }

                            try
                            {
                                await context.SaveChangesAsync();
                                TempData["SuccessMessage"] = "Nhập dữ liệu thành công!";
                            }
                            catch (Exception ex)
                            {
                                TempData["ErrorMessage"] = $"Lỗi khi lưu dữ liệu: {ex.Message}";
                            }

                            return RedirectToAction("ViewImport");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("ViewImport");
            }
        }
    }

}
