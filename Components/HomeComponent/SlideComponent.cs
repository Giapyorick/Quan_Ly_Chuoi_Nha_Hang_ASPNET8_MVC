using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GiapDepTrai.Models;

namespace GiapDepTrai.Components
{
    [ViewComponent (Name = "Silde")]
    public class SlideComponent:ViewComponent
    {
        private readonly QlChuoiNhaHang1Context _context;
        public SlideComponent(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slide = (from x in _context.TblSlideHomes
                         where x.IsActive == true
                         orderby x.Slide1Id
                         select x).Take(1).ToList();
            return await Task.FromResult((IViewComponentResult)View("ViewSilde", slide));
        }
    }
}
