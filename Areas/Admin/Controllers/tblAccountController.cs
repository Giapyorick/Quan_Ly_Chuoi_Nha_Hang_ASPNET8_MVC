using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiapDepTrai.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblAccountController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public tblAccountController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public void LayDl()
        {
            var employee = _context.TblEmployees
                .Select(s => new SelectListItem
                {
                    
                    Value = s.EmployeeId.ToString(),
                    Text = s.FullName

                });
            ViewBag.Account_Employee = new SelectList(employee, "Value", "Text");

            
        }
        [HttpGet]
        public IActionResult Add()
        {
            LayDl();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddNewAccount account)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                }
                return View(account);
            }
            var Md5Password = Functions.MD5Password(account.PassWords);
            var item = new TblAccount
            {
  
                UserName = account.UserName,
                PassWords = Md5Password,
                EmployeeId = account.EmployeeId,
                Roles = account.Roles

            };

            await _context.TblAccounts.AddAsync(item);
            await _context.SaveChangesAsync();
            LayDl();
            return RedirectToAction("List", new { page = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchAccount, int page = 1, int pageSize = 5)
        {
            var query = _context.TblAccounts.AsQueryable();
            /*ViewBag.Rolec = HttpContext.Session.GetString("Role");*/
            if (!string.IsNullOrWhiteSpace(searchAccount))
            {
                query = query.Where(s =>
                    (!string.IsNullOrEmpty(s.UserName) && s.UserName.Contains(searchAccount)) ||
                    (!string.IsNullOrEmpty(s.Roles)&&s.Roles.Contains(searchAccount))||
                    (s.EmployeeId.HasValue && s.EmployeeId.Value.ToString().Contains(searchAccount))
                );
            }
            int totalItems = await query.CountAsync();
            var acc = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPagesAccount = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPagesAccount = page;
            LayDl();
            return View(acc);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var acc = await _context.TblAccounts.FindAsync(id);
            LayDl();
            return View(acc);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TblAccount acc)
        {
            var item = await _context.TblAccounts.FindAsync(acc.AccountId);
            if (item is not null)
            {
                item.AccountId = acc.AccountId;
                item.UserName = acc.UserName;
                item.PassWords = acc.PassWords;
                item.EmployeeId = acc.EmployeeId;
                item.Roles = acc.Roles;

                await _context.SaveChangesAsync();
            }
            LayDl();
            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(TblAccount acc)
        {
            try
            {
                var item = await _context.TblAccounts.AsNoTracking().FirstOrDefaultAsync(b => b.AccountId == acc.AccountId);
                if (item is not null)
                {
                    _context.TblAccounts.Remove(item);
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
