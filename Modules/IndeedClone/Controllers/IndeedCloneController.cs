using IndeedClone.Modules.IndeedClone.DTO;
using IndeedClone.Modules.IndeedClone.Models;
using IndeedClone.Modules.IndeedClone.ServiceContracts;
using Microsoft.AspNetCore.Mvc;


namespace IndeedClone.Modules.IndeedClone.Controllers
{
    public class IndeedCloneController : Controller
    {
        private readonly IIndeedCloneHomeService _indeedCloneHomeService;
        private readonly IIndeedCloneJobDescriptionService _indeedCloneJobDescriptionService;

        public IndeedCloneController(IIndeedCloneHomeService indeedCloneHomeService, IIndeedCloneJobDescriptionService indeedCloneJobDescriptionService)
        {
            _indeedCloneHomeService = indeedCloneHomeService;
            _indeedCloneJobDescriptionService = indeedCloneJobDescriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Retrieve(IndeedCloneJobSearchFilterDTO filters, int page = 1)
        {
            const int pageSize = 10;
            filters.Page = page;
            var paginatedResult = await _indeedCloneHomeService.GetFilteredJobsAsync(filters, pageSize);

            var param = new IndeedCloneHomeViewModel
            {
                LeftJobCards = paginatedResult.Jobs,
                RightJobDetails = null,
                Pagination = paginatedResult.Pagination,
                SearchFilter = filters
            };

            return View("Home", param);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobsPage(IndeedCloneJobSearchFilterDTO filters, int page = 1)
        {
            const int pageSize = 10;
            filters.Page = page;
            var paginatedResult = await _indeedCloneHomeService.GetFilteredJobsAsync(filters, pageSize);

            // Return JSON instead of partial view
            return Json(paginatedResult.Jobs);
        }

        [HttpGet]
        public async Task<IActionResult> FullJobDescription(string? jobUid)
        {
            if (string.IsNullOrEmpty(jobUid))
                return BadRequest("Invalid JobUid");

            var job = await _indeedCloneJobDescriptionService.GetJobDescriptionAsync(jobUid);
            if (job == null)
                return NotFound();

            return PartialView("_FullJobDescriptionPartial", job);
        }

        [HttpPost]
        public async Task<IActionResult> GetNoResultsPartial([FromBody] IndeedCloneJobSearchFilterDTO filters)
        {
            return PartialView("_NoResultsPartial", filters);
        }

        [HttpGet]
        public async Task<IActionResult> CompanyReviews()
        {
            return View("companyreviews");
        }

        [HttpGet]
        public async Task<IActionResult> Salaries()
        {
            return View("salary");
        }

    }
}
