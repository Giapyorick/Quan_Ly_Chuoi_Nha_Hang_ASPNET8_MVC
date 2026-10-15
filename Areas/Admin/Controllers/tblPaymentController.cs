using GiapDepTrai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class tblPaymentController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;
        public tblPaymentController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

    }
}
