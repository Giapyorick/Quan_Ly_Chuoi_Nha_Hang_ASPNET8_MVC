using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Models;

namespace GiapDepTrai.Components
{
    [ViewComponent (Name = "Menu")]
    public class MenuComponent: ViewComponent
    {
        private readonly QlChuoiNhaHang1Context _context;
        public MenuComponent(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listMenu = (from m in _context.TblMenus
                            where (m.IsActive == true) && (m.Position == 1)
                            select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("ViewMenu", listMenu));
        }
    }
}
