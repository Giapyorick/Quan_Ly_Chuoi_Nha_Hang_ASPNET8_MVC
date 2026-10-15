using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GiapDepTrai.Controllers
{
    public class ContactController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public ContactController(QlChuoiNhaHang1Context context){
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AddNewContact contact)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return View(contact);
            }
            
            var item = new TblMessageContact
            {
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Message = contact.Message,
                DateTime = DateTime.Now
            };

            await _context.TblMessageContacts.AddAsync(item);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Gửi liên hệ thành công!";
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}