using Microsoft.AspNetCore.Mvc;
using GiapDepTrai.Models;

namespace GiapDepTrai.Components
{
    [ViewComponent(Name =("Story"))]
    public class StoryComponent : ViewComponent
    {
        private readonly QlChuoiNhaHang1Context _context;
        public StoryComponent(QlChuoiNhaHang1Context context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var story = (from s in _context.TblStoryHomes
                         where s.IsActive == true
                         orderby s.StoryId
                         select s).Take(1).ToList();
            return await Task.FromResult((IViewComponentResult)View("ViewStory", story));
        }
    }
}
