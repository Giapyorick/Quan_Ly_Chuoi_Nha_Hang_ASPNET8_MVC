using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Models;

namespace QL_Nha_Hang.Areas.Admin.Component
{
    [ViewComponent(Name = "AdminMenu")]
    public class AdminMenuComponents : ViewComponent
    {
        private readonly QlChuoiNhaHang1Context _context;
        public AdminMenuComponents(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mnList = (from mn in _context.TblAdminMenus
                          where mn.IsActive == true
                          select mn).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", mnList));
        }

    }

}
