using IndeedClone.Modules.IndeedClone.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IndeedClone.Modules.IndeedClone.ViewComponents
{
    public class FilterJobDropdownsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IndeedCloneJobSearchFilterDTO? filters)
        {
            return View("~/Modules/IndeedClone/Views/Home/FilterJobDropdowns.cshtml", filters ?? new IndeedCloneJobSearchFilterDTO());
        }
    }
}
