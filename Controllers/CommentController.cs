using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Models;

public class CommentController : Controller
{
    private readonly QlChuoiNhaHang1Context _context;

    public CommentController(QlChuoiNhaHang1Context context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Add(CommentViewModel model)
    {
        if (ModelState.IsValid)
        {
			var customerId = HttpContext.Session.GetInt32("CustomerID");
            var comment = new TblComment
            {
                Contents = model.Contents,
                CommentDate = DateTime.Now,
                MenuItemId = model.MenuItemId,
                CustomerId = customerId
            };

            _context.TblComments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", "MenuItem", new { id = model.MenuItemId });
        }

        return View(model); 
    }
}
