using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using GiapDepTrai.Utilities;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblCustommerController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        private readonly IWebHostEnvironment _iwebHostEnvironment;
        public tblCustommerController( QlChuoiNhaHang1Context context, IWebHostEnvironment iwebHostEnvironment)
        {
            _context = context;
            _iwebHostEnvironment = iwebHostEnvironment;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddNewCustommer custommer)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    
                        TempData["ErrorMessage"] = error.ErrorMessage;
                }
                return View(custommer);
            }
            if (custommer.formFile != null && custommer.formFile.Length > 0)
            {
                string Diachitep = Path.Combine(_iwebHostEnvironment.WebRootPath, "assets", "images", "Customer");
                if (!Directory.Exists(Diachitep))
                {
                    Directory.CreateDirectory(Diachitep);
                }
                string tenAnh = $"Custommer_{Guid.NewGuid()}.jpg";
                string diachiAnh = Path.Combine(Diachitep, tenAnh);
                using (var stream = new FileStream(diachiAnh, FileMode.Create))
                {
                    await custommer.formFile.CopyToAsync(stream);
                }
                custommer.Img = $"assets/images/Customer/{tenAnh}";
            }
            
            var curentdate = DateTime.Now;
            var item = new TblCustomer
            {
                FullName = custommer.FullName,
                Gender = custommer.Gender,
                Address = custommer.Address,
                Phone = custommer.Phone,
                Email = custommer.Email,
                CreatedAt = curentdate,
                IdentityCard = custommer.IdentityCard,
                BirthofDate = custommer.BirthofDate,
                Img = custommer.Img,
                Status = custommer.Status,
                Passwork = Functions.MD5Password(custommer.Passwork)

            };

            await _context.TblCustomers.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("List", new { page = 1 });
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchCustomer, int page = 1, int pageSize = 5)
        {
            var query = _context.TblCustomers.AsQueryable();
            /*ViewBag.Rolec = HttpContext.Session.GetString("Role");*/
            if (!string.IsNullOrWhiteSpace(searchCustomer))
            {
                query = query.Where(s =>
                    (!string.IsNullOrEmpty(s.FullName) && s.FullName.Contains(searchCustomer)) ||
                    (!string.IsNullOrEmpty(s.Address) && s.Address.Contains(searchCustomer)) ||
                    (!string.IsNullOrEmpty(s.Email) && s.Email.Contains(searchCustomer))
                );
            }
            int totalItems = await query.CountAsync();
            var cunstomer = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesCustomer = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPageCustomer = page;

            return View(cunstomer);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.TblCustomers.FindAsync(id);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblCustomer customer, IFormFile? formFile)
        {
            var item = await _context.TblCustomers.FindAsync(customer.CustomerId);
            if (item is null)
            {
                return NotFound();
            }
            item.FullName = customer.FullName;
            item.Gender = customer.Gender;
            item.Address = customer.Address;
            item.Phone = customer.Phone;
            item.Email = customer.Email;
            item.IdentityCard = customer.IdentityCard;
            item.BirthofDate = customer.BirthofDate;
            item.Status = customer.Status;
            item.Passwork = customer.Passwork;

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

                string diachitep = Path.Combine(_iwebHostEnvironment.WebRootPath, "assets", "images", "Customer");
                if (!Directory.Exists(diachitep))
                {
                    Directory.CreateDirectory(diachitep);
                }
                string tenAnh = $"Customer_{Guid.NewGuid()}.jpg";
                string diachiAnh = Path.Combine(diachitep, tenAnh);
                using (var stream = new FileStream(diachiAnh, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                item.Img = $"assets/images/Customer/{tenAnh}";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblCustomer customer)
        {
            try
            {
                var item = await _context.TblCustomers.AsNoTracking().FirstOrDefaultAsync(b => b.CustomerId == customer.CustomerId);
                if (item is not null)
                {
                    if (!string.IsNullOrEmpty(customer.Img))
                    {
                        string diachiAnh = Path.Combine(_iwebHostEnvironment.WebRootPath, customer.Img.TrimStart('/'));
                        if (System.IO.File.Exists(diachiAnh))
                        {
                            System.IO.File.Delete(diachiAnh);
                        }
                    }
                    _context.TblCustomers.Remove(item);
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
            List<TblCustomer> list;
            using (var context = new QlChuoiNhaHang1Context())
            {
                list = await context.TblCustomers.ToListAsync();
            }

            var curentdate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("tblCustommer");

                worksheet.Cells["A1"].Value = "Mã khách hàng";
                worksheet.Cells["B1"].Value = "Tên khách hàng";
                worksheet.Cells["C1"].Value = "Giới tính";
                worksheet.Cells["D1"].Value = "Số điện thoại";
                worksheet.Cells["E1"].Value = "Địa chỉ";
                worksheet.Cells["F1"].Value = "Thư điện tử";
                worksheet.Cells["G1"].Value = "Ngày sinh";
                worksheet.Cells["H1"].Value = "Thẻ căn cước";
                worksheet.Cells["I1"].Value = "Ảnh";
                worksheet.Cells["J1"].Value = "Trạng thái";
                worksheet.Cells["K1"].Value = "Ngày tạo tài khoản";

                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = list[i].CustomerId;
                    worksheet.Cells[i + 2, 2].Value = list[i].FullName;
                    worksheet.Cells[i + 2, 3].Value = list[i].Gender;
                    worksheet.Cells[i + 2, 4].Value = list[i].Phone;
                    worksheet.Cells[i + 2, 5].Value = list[i].Address;
                    worksheet.Cells[i + 2, 6].Value = list[i].Email;
                    worksheet.Cells[i + 2, 7].Value = list[i].BirthofDate.ToString();
                    worksheet.Cells[i + 2, 8].Value = list[i].IdentityCard;
                    worksheet.Cells[i + 2, 9].Value = list[i].Img;
                    worksheet.Cells[i + 2, 10].Value = list[i].Status;
                    worksheet.Cells[i + 2, 11].Value = list[i].CreatedAt.ToString();
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream(await package.GetAsByteArrayAsync());
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"tblCustomer{curentdate}.xlsx");
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
                                var customerName = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                                var gender = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                                var phone = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                                var address = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                                var email = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                                var date = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
                                var identityCard = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
                                var img = worksheet.Cells[row, 8].Value?.ToString()?.Trim();
                                var status = worksheet.Cells[row, 9].Value?.ToString()?.Trim();
                                if (string.IsNullOrEmpty(customerName))
                                {
                                    TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Thiếu tên khách hàng (dữ liệu bắt buộc).";
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
                                        TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Giá trị 'Status' không hợp lệ ({status}).";
                                        continue; 
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Thiếu giá trị 'Status'.";
                                    continue; 
                                }
                                DateOnly? birthDate = null;
                                if (!string.IsNullOrEmpty(date))
                                {
                                    try
                                    {
                                        if (DateTime.TryParse(date, out DateTime parsedDate))
                                        {
                                            birthDate = DateOnly.FromDateTime(parsedDate);
                                        }
                                        else
                                        {
                                            var datePart = date.Length >= 10 ? date.Substring(0, 10) : date;
                                            birthDate = DateOnly.Parse(datePart);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Ngày sinh không hợp lệ ({date}). Lỗi: {ex.Message}";
                                        continue; 
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = $"Bỏ qua dòng {row}: Thiếu ngày sinh.";
                                    continue;
                                }

                               
                                var tblCustomer = new TblCustomer
                                {
                                    FullName = customerName,
                                    Gender = gender ?? "Nam",
                                    Phone = phone ?? "null",
                                    Address = address ?? "null",
                                    Email = email,
                                    BirthofDate = birthDate.Value, 
                                    IdentityCard = identityCard,
                                    Img = img,
                                    Status = statusValue.Value,
                                    CreatedAt = DateTime.Now
                                    
                                };

                                context.TblCustomers.Add(tblCustomer);
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
