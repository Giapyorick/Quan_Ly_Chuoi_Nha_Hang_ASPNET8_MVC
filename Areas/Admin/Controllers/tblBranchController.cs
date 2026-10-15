using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiapDepTrai.Models;
using System.ComponentModel;
using System.Formats.Asn1;
using OfficeOpenXml;


namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblBranchController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public tblBranchController(QlChuoiNhaHang1Context context) 
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddNewBranch branch)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["ErrorMessage"] = "ModelState Error: " + error.ErrorMessage;
                }
                return View(branch);
            }

            var item = new TblBranch
            {
                BranchName = branch.BranchName,
                AddressBranch = branch.AddressBranch,
                Phone = branch.Phone,
                StatusBranch = branch.StatusBranch,
            };

            await _context.TblBranches.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("List", new { page = 1 });
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchBranch, int page = 1, int pageSize = 5)
        {
            var query = _context.TblBranches.AsQueryable();
            /*ViewBag.Rolec = HttpContext.Session.GetString("Role");*/
            if (!string.IsNullOrWhiteSpace(searchBranch))
            {
                query = query.Where(s =>
                    (!string.IsNullOrEmpty(s.BranchName) && s.BranchName.Contains(searchBranch)) ||
                    (!string.IsNullOrEmpty(s.AddressBranch) && s.AddressBranch.Contains(searchBranch))
                );
            }
            int totalItems = await query.CountAsync();
            var branch = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesC = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageC = page;

            return View(branch);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _context.TblBranches.FindAsync(id);
            return View(branch);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblBranch brach)
        {
            var item = await _context.TblBranches.FindAsync(brach.BranchId);
            if (item is not null)
            {
                item.BranchName = brach.BranchName;
                item.AddressBranch = brach.AddressBranch;
                item.Phone = brach.Phone;
                item.StatusBranch = brach.StatusBranch;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblBranch branch)
        {
            try
            {
                var item = await _context.TblBranches.AsNoTracking().FirstOrDefaultAsync(b => b.BranchId == branch.BranchId);
                if (item is not null)
                {
                    _context.TblBranches.Remove(item);
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
        [HttpGet]
        public IActionResult ImportExcel()
        {
            return View();
        }

        
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                List<TblBranch> list;
                using (var context = new QlChuoiNhaHang1Context())
                {
                    list = await context.TblBranches.ToListAsync();
                }

                var curentdate = DateTime.Now.ToString("yyyyMMdd_HHmmss");


                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("TblBranch");

                    worksheet.Cells["A1"].Value = "Mã chi nhánh";
                    worksheet.Cells["B1"].Value = "Tên chi nhánh";
                    worksheet.Cells["C1"].Value = "Địa chỉ";
                    worksheet.Cells["D1"].Value = "Số điện thoại";
                    worksheet.Cells["E1"].Value = "Trạng thái";

                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = list[i].BranchId;
                        worksheet.Cells[i + 2, 2].Value = list[i].BranchName;
                        worksheet.Cells[i + 2, 3].Value = list[i].AddressBranch;
                        worksheet.Cells[i + 2, 4].Value = list[i].Phone;
                        worksheet.Cells[i + 2, 5].Value = list[i].StatusBranch;
                    }

                    worksheet.Cells.AutoFitColumns();

                    var stream = new MemoryStream(await package.GetAsByteArrayAsync());
                    stream.Position = 0;
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"TblBranch{curentdate}.xlsx");
                }
           
            }
            catch(Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }

        }
    }
}
