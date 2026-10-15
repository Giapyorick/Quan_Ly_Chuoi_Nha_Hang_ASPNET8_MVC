using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Models;

namespace GiapDepTrai.Components
{
    [ViewComponent(Name = "Navigation")]
    public class NavigationComponent : ViewComponent
    {
        private readonly QlChuoiNhaHang1Context _context;

        public NavigationComponent(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var nav = (from n in _context.TblNavigationHomes
                       where n.IsActive == true
                       orderby n.NavigationId
                       select n).Take(1).ToList();
            return await Task.FromResult((IViewComponentResult)View("ViewNavigation", nav));
        }

    }
}
