using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GiapDepTrai.Models;
using GiapDepTrai1.Models;
using System.Text.Encodings.Web;
using System.Collections.Generic;

namespace GiapDepTrai.Controllers
{
    public class CartController : Controller
    {
        private readonly QlChuoiNhaHang1Context _context;

        public CartController(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

        public class AddToCartRequest
        {
            public string? UserId { get; set; }
            public int MenuItemId { get; set; }
            public int Quantity { get; set; }
            public string? Note { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerID");
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = JavaScriptEncoder.Default.Encode("Dữ liệu không hợp lệ.");
                return Redirect("http://localhost:5214/MenuItem");
            }

            try
            {
                var cart = await _context.TblCarts
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (cart == null)
                {
                    cart = new TblCart
                    {
                        CustomerId = customerId,
                        CreatedDate = DateTime.Now
                    };
                    _context.TblCarts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                var menuItem = await _context.TblMenuItems
                    .FirstOrDefaultAsync(m => m.MenuItemId == request.MenuItemId);

                if (menuItem == null)
                {
                    TempData["ErrorMessage"] = JavaScriptEncoder.Default.Encode("Món ăn không tồn tại.");
                    return Redirect("http://localhost:5214/MenuItem");
                }

                var cartItem = await _context.TblCartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.MenuItemId == request.MenuItemId);

                if (cartItem != null)
                {
                    cartItem.Quantity += request.Quantity;
                    cartItem.Note = request.Note;
                    cartItem.Price = menuItem.Price ?? 0;
                }
                else
                {
                    var newCartItem = new TblCartItem
                    {
                        CartId = cart.CartId,
                        MenuItemId = request.MenuItemId,
                        Quantity = request.Quantity,
                        Price = menuItem.Price ?? 0,
                        Note = request.Note
                    };
                    _context.TblCartItems.Add(newCartItem);
                }

                await _context.SaveChangesAsync();
                if (customerId is not null)
                {
                    var count_cart = await _context.TblCartItems
                        .Include(s => s.Cart)
                        .CountAsync(c => c.Cart.CustomerId == customerId);
                    HttpContext.Session.SetInt32("Count_Cart", count_cart);
                }

                TempData["SuccessMessage"] = JavaScriptEncoder.Default.Encode("Thêm món vào giỏ hàng thành công!");
                return Redirect("http://localhost:5214/MenuItem");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = JavaScriptEncoder.Default.Encode($"Lỗi khi thêm vào giỏ hàng: {ex.Message}");
                return Redirect("http://localhost:5214/MenuItem");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewCart()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerID");
            if (customerId == null)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return RedirectToAction("Login", "Login", new { returnUrl = currentUrl });
            }

            var userId = customerId;

            var cart = await _context.TblCarts
                .Include(c => c.CartItem)
                .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.CustomerId == userId);


            if (cart == null || cart.CartItem == null || !cart.CartItem.Any())
            {
                ViewBag.Message = "Giỏ hàng trống.";
                return View(new List<TblCartItem>());
            }
            if (customerId is not null)
            {
                var count_cart = await _context.TblCartItems
                    .Include(s => s.Cart)
                    .CountAsync(c => c.Cart.CustomerId == customerId);
                HttpContext.Session.SetInt32("Count_Cart", count_cart);
            }
            var cartItems = cart.CartItem.ToList();
            return View(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart(List<TblCartItem> CartItems)
        {
            if (CartItems == null || !CartItems.Any())
            {
                TempData["ErrorMessage"] = JavaScriptEncoder.Default.Encode("Giỏ hàng trống!");
                return RedirectToAction("ViewCart");
            }

            foreach (var item in CartItems)
            {
                var cartItem = await _context.TblCartItems.FindAsync(item.CartItemId);
                if (cartItem != null)
                {
                    if (item.Quantity > 0)
                    {
                        cartItem.Quantity = item.Quantity;
                        cartItem.Note = item.Note;
                    }
                    else
                    {
                        _context.TblCartItems.Remove(cartItem); 
                    }
                }
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = JavaScriptEncoder.Default.Encode("Cập nhật giỏ hàng thành công!");
            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.TblCartItems.FindAsync(cartItemId);

            if (cartItem != null)
            {
                _context.TblCartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = JavaScriptEncoder.Default.Encode("Đã xóa món khỏi giỏ hàng!");
                var customerId = HttpContext.Session.GetInt32("CustomerID");
                if (customerId is not null)
                {
                    var count_cart = await _context.TblCartItems
                        .Include(s => s.Cart)
                        .CountAsync(c => c.Cart.CustomerId == customerId);
                    HttpContext.Session.SetInt32("Count_Cart", count_cart);
                }
            }
            else
            {
                TempData["ErrorMessage"] = JavaScriptEncoder.Default.Encode("Món cần xóa không tồn tại!");
            }
            return RedirectToAction("ViewCart");
        }


    }
}
