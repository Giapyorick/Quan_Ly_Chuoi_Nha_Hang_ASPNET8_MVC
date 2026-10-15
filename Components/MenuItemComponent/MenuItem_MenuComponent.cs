using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Models;

namespace GiapDepTrai.Components
{
	[ViewComponent(Name ="MenuItem_Menu")]
    public class MenuItemComponent:ViewComponent
    {
        private readonly QlChuoiNhaHang1Context _context;
        public MenuItemComponent(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var itemMenu = (from i in _context.TblMenuItems
                            where i.IsAvailable == true
                            orderby i.Count_Saled descending
                            select i).ToList();
            return await Task.FromResult((IViewComponentResult)View("ViewMenuItem_Menu", itemMenu));
        }
    }
}
