using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TblEmployeeController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        private readonly IWebHostEnvironment _iwebHostEnvironment;
        public TblEmployeeController(QlChuoiNhaHang1Context context, IWebHostEnvironment iwebHostEnvironment)
        {
            _context = context;
            _iwebHostEnvironment = iwebHostEnvironment;
        }
        public void LayDL()
        {
            var employee = _context.TblBranches
                .Select(t => new SelectListItem
                {
                    Value = t.BranchId.ToString(),
                    Text = t.BranchName
                }).ToList();
            ViewBag.Employees = new SelectList(employee, "Value", "Text");
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
        public async Task<IActionResult> Add(AddNewEmployee employee)
        {
            LayDL();
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["ErrorMessage"] = error.ErrorMessage;
                }
                return View(employee);
            }
            if (employee.formFile != null && employee.formFile.Length > 0)
                {
                    string Diachitep = Path.Combine(_iwebHostEnvironment.WebRootPath, "assets", "images", "Employee");
                    if (!Directory.Exists(Diachitep))
                    {
                        Directory.CreateDirectory(Diachitep);
                    }
                    string tenAnh = $"Employee_{Guid.NewGuid()}.jpg";
                    string diachiAnh = Path.Combine(Diachitep, tenAnh);
                    using (var stream = new FileStream(diachiAnh, FileMode.Create))
                    {
                        await employee.formFile.CopyToAsync(stream);
                    }
                    employee.Img = $"assets/images/Employee/{tenAnh}";
                }
            var curentDate = DateTime.Now;
            var item = new TblEmployee
            {
                FullName = employee.FullName,
                Phone = employee.Phone,
                Position = employee.Position,
                BranchId = employee.BranchId, 
                CreatedAt = curentDate,
                Gender = employee.Gender,
                DateofBirth = employee.DateofBirth,
                IdentityCard = employee.IdentityCard,
                Addresss = employee.Addresss,
                Img = employee.Img,
                Status = employee.Status
            };
            await _context.TblEmployees.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("List", new { page = 1 });
        }
        [HttpGet]
        public async Task<IActionResult> List( string? searchEmployee, int page = 1, int pageSize =5)
        {
            LayDL();

            var employee =  _context.TblEmployees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchEmployee))
            {
                employee = employee.Where(e => 
                                (!string.IsNullOrEmpty(e.FullName) && e.FullName.Contains(searchEmployee)) ||
                                (!string.IsNullOrEmpty(e.IdentityCard)&&e.IdentityCard.Contains(searchEmployee))||
                                (!string.IsNullOrEmpty(e.Phone)&&e.Phone.Contains(searchEmployee))||
                                (!string.IsNullOrEmpty(e.Gender)&& e.Gender.Contains(searchEmployee))||
                                (!string.IsNullOrEmpty(e.Position) && e.Position.Contains(searchEmployee)) ||
                                (e.BranchId.HasValue && e.BranchId.Value.ToString().Contains(searchEmployee)));
            }
            int totalItems = await employee.CountAsync();
            var employees = await employee
                .Include(e => e.Branch)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesEmpolyee = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageEmployee = page;

            return View(employees);
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
            LayDL();
            var item = await _context.TblEmployees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblEmployee employee, IFormFile? formFile)
        {
            var item = await _context.TblEmployees.FindAsync(employee.EmployeeId);
            if (item is null)
            {
                return NotFound();
            }
            else
            {
                item.EmployeeId = employee.EmployeeId;
                item.FullName = employee.FullName;
                item.Phone = employee.Phone;
                item.Addresss = employee.Addresss;
                item.Gender = employee.Gender;
                item.DateofBirth = employee.DateofBirth;
                item.IdentityCard = employee.IdentityCard;
                item.CreatedAt = employee.CreatedAt;
                item.BranchId = employee.BranchId;
                item.Status = employee.Status;
                if (formFile != null && formFile.Length > 0)
                {
                if (!string.IsNullOrEmpty(item.Img))
                {
                    string oldImagePath = Path.Combine(_iwebHostEnvironment.WebRootPath, item.Img.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string diachitep = Path.Combine(_iwebHostEnvironment.WebRootPath, "assets", "images", "Employee");
                if (!Directory.Exists(diachitep))
                {
                    Directory.CreateDirectory(diachitep);
                }
                string tenAnh = $"Employee_{Guid.NewGuid()}.jpg";
                string diachiAnh = Path.Combine(diachitep, tenAnh);
                using (var stream = new FileStream(diachiAnh, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                item.Img = $"assets/images/Employee/{tenAnh}";
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Chỉnh sửa thông tin nhân viên thành công";

            }
            LayDL();
            return (RedirectToAction("List", new { page = 1 }));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblEmployee employee)
        {
            try
            {
                var item = await _context.TblEmployees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
                if (item is not null)
                {
                    if (!string.IsNullOrEmpty(employee.Img))
                    {
                        string diachiAnh = Path.Combine(_iwebHostEnvironment.WebRootPath, employee.Img.TrimStart('/'));
                        if (System.IO.File.Exists(diachiAnh))
                        {
                            System.IO.File.Delete(diachiAnh);
                        }
                    }
                    _context.TblEmployees.Remove(item);
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
            if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Vui lòng chọn tệp Excel (.xlsx).";
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
                        var rowCount = worksheet?.Dimension?.Rows ?? 0;

                        if (rowCount < 2)
                        {
                            TempData["ErrorMessage"] = "Tệp Excel không chứa dữ liệu hợp lệ.";
                            return RedirectToAction("ViewImport");
                        }

                        var errors = new List<string>();
                        var employeesToAdd = new List<TblEmployee>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                var employeeName = worksheet?.Cells[row, 1].Value?.ToString()?.Trim();
                                var phone = worksheet?.Cells[row, 2].Value?.ToString()?.Trim();
                                var position = worksheet?.Cells[row, 3].Value?.ToString()?.Trim();
                                var branchIdStr = worksheet?.Cells[row, 4].Value?.ToString()?.Trim();
                                var gender = worksheet?.Cells[row, 6].Value?.ToString()?.Trim();
                                var date = worksheet?.Cells[row, 7].Value?.ToString()?.Trim();
                                var identity = worksheet?.Cells[row, 8].Value?.ToString()?.Trim();
                                var address = worksheet?.Cells[row, 9].Value?.ToString()?.Trim();
                                var status = worksheet?.Cells[row, 10].Value?.ToString()?.Trim();

                                if (string.IsNullOrEmpty(employeeName))
                                {
                                    errors.Add($"Dòng {row}: Thiếu tên nhân viên.");
                                    continue;
                                }

                                bool? statusValue = null;
                                if (!string.IsNullOrEmpty(status))
                                {
                                    if (status.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                                        status.Equals("1") ||
                                        status.Equals("Hoạt động", StringComparison.OrdinalIgnoreCase))
                                    {
                                        statusValue = true;
                                    }
                                    else if (status.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                                             status.Equals("0") ||
                                             status.Equals("Không hoạt động", StringComparison.OrdinalIgnoreCase))
                                    {
                                        statusValue = false;
                                    }
                                    else
                                    {
                                        errors.Add($"Dòng {row}: Giá trị 'Status' không hợp lệ ({status}).");
                                        continue;
                                    }
                                }
                                else
                                {
                                    errors.Add($"Dòng {row}: Thiếu giá trị 'Status'.");
                                    continue;
                                }

                                DateOnly? birthDate = null;
                                if (!string.IsNullOrEmpty(date))
                                {
                                    string format = "dd/MM/yyyy";
                                    if (DateTime.TryParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                                    {
                                        birthDate = DateOnly.FromDateTime(parsedDate);
                                    }
                                    else
                                    {
                                        errors.Add($"Dòng {row}: Ngày sinh không hợp lệ ({date}). Định dạng đúng là {format}.");
                                        continue;
                                    }
                                }
                                else
                                {
                                    errors.Add($"Dòng {row}: Thiếu ngày sinh.");
                                    continue;
                                }

                                int? branchIdValue = null;
                                if (!string.IsNullOrEmpty(branchIdStr) && int.TryParse(branchIdStr, out int parsedBranchId))
                                {
                                    if (_context.TblBranches.Any(b => b.BranchId == parsedBranchId))
                                    {
                                        branchIdValue = parsedBranchId;
                                    }
                                    else
                                    {
                                        errors.Add($"Dòng {row}: Mã chi nhánh không tồn tại ({branchIdStr}).");
                                        continue;
                                    }
                                }

                                var tblEmployee = new TblEmployee
                                {
                                    FullName = employeeName,
                                    Phone = "0"+phone,
                                    Position = position ?? "null",
                                    BranchId = branchIdValue,
                                    CreatedAt = DateTime.Now,                                    
                                    Gender = gender,
                                    DateofBirth = birthDate.Value,
                                    IdentityCard = identity,
                                    Addresss = address,
                                    Status = statusValue.Value,
                                    
                                };

                                employeesToAdd.Add(tblEmployee);
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"Dòng {row}: Lỗi xử lý dữ liệu: {ex.Message}");
                            }
                        }

                        if (errors.Any())
                        {
                            TempData["ErrorMessage"] = string.Join("<br>", errors.Take(5));
                            if (errors.Count > 5)
                            {
                                TempData["ErrorMessage"] += $"<br>Còn {errors.Count - 5} lỗi khác. Vui lòng kiểm tra file Excel.";
                            }
                        }

                        if (employeesToAdd.Any())
                        {
                            try
                            {
                                _context.TblEmployees.AddRange(employeesToAdd);
                                await _context.SaveChangesAsync();
                                TempData["SuccessMessage"] = $"Nhập thành công {employeesToAdd.Count} nhân viên!";
                            }
                            catch (Exception ex)
                            {
                                var inner = ex.InnerException?.Message ?? ex.Message;
                                TempData["ErrorMessage"] = $"Lỗi khi lưu dữ liệu: {ex.Message},<> {inner}";
                            }
                        }
                        else if (!errors.Any())
                        {
                            TempData["ErrorMessage"] = "Không có dữ liệu hợp lệ để nhập.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi xử lý file Excel: {ex.Message}";
            }

            return RedirectToAction("ViewImport");
        }

        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var employees = await _context.TblEmployees.ToListAsync();
                var currentDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("TblEmployee");

                    worksheet.Cells[1, 1].Value = "Tên nhân viên";
                    worksheet.Cells[1, 2].Value = "SĐT";
                    worksheet.Cells[1, 3].Value = "Vị trí";
                    worksheet.Cells[1, 4].Value = "Mã chi nhánh";
                    worksheet.Cells[1, 5].Value = "Ngày tạo";
                    worksheet.Cells[1, 6].Value = "Giới tính";
                    worksheet.Cells[1, 7].Value = "Ngày sinh";
                    worksheet.Cells[1, 8].Value = "Thẻ căn cước";
                    worksheet.Cells[1, 9].Value = "Địa chỉ";
                    worksheet.Cells[1, 10].Value = "Trạng thái";

                    for (int i = 0; i < employees.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = employees[i].FullName ?? "";
                        worksheet.Cells[i + 2, 2].Value = employees[i].Phone ?? "";
                        worksheet.Cells[i + 2, 3].Value = employees[i].Position ?? "";
                        worksheet.Cells[i + 2, 4].Value = employees[i].BranchId.HasValue ? employees[i].BranchId?.ToString() : "";
                        worksheet.Cells[i + 2, 5].Value = employees[i].CreatedAt.HasValue ? employees[i].CreatedAt?.ToString("dd/MM/yyyy HH:mm:ss") : "";
                        worksheet.Cells[i + 2, 6].Value = employees[i].Gender ?? "Nam";
                        worksheet.Cells[i + 2, 7].Value = employees[i].DateofBirth.HasValue ? employees[i].DateofBirth?.ToString("dd/MM/yyyy") : "";
                        worksheet.Cells[i + 2, 8].Value = employees[i].IdentityCard ?? "";
                        worksheet.Cells[i + 2, 9].Value = employees[i].Addresss;
                        worksheet.Cells[i + 2, 10].Value = employees[i].Status is bool status? (status ? "Hoạt động" : "Không hoạt động"): "Chưa rõ";
                    }

                    worksheet.Cells.AutoFitColumns();

                    var stream = new MemoryStream(await package.GetAsByteArrayAsync());
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"TblEmployee_{currentDate}.xlsx");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi xuất file Excel: {ex.Message}";
                return RedirectToAction("ViewImport");
            }
        }
    }
}
