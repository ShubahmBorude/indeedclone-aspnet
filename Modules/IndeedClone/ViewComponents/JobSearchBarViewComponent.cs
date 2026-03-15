using IndeedClone.Modules.IndeedClone.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IndeedClone.Modules.IndeedClone.ViewComponents
{
    public class JobSearchBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IndeedCloneJobSearchFilterDTO? filters)
        {
            return View("~/Modules/IndeedClone/Views/Home/JobSearchBar.cshtml", filters ?? new IndeedCloneJobSearchFilterDTO());
        }
    }
}
