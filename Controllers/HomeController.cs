using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Models;
using Microsoft.EntityFrameworkCore;

namespace GiapDepTrai.Controllers;

public class HomeController : Controller
{
    private readonly QlChuoiNhaHang1Context _context;
    public HomeController(QlChuoiNhaHang1Context context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var customerId = HttpContext.Session.GetInt32("CustomerID");
        if (customerId is not null)
        {
            var count_cart = await _context.TblCartItems
                .Include(s => s.Cart)
                .CountAsync(c => c.Cart.CustomerId == customerId);
            HttpContext.Session.SetInt32("Count_Cart", count_cart);
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
