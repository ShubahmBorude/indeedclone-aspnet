using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.Shared.Universal;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Locations;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace IndeedClone.Modules.SubModules.JobPost.Controllers
{
    public class JobPostController : Controller
    {
        private static readonly Dictionary<int, string> PageActionMap = new()
        { 
            { (int)JobPages.JobOrganization, "Retrieve" }, { (int)JobPages.JobBasics, "JobBasics" }, { (int)JobPages.JobDetails, "JobDeatils" }, 
            { (int)JobPages.JobPayBenefits, "PayBenefits" }, { (int)JobPages.JobPreferences, "JobPreferences" }, { (int)JobPages.JobDescription, "JobDescription" }, 
            { (int)JobPages.Review, "Review" }, { (int)JobPages.JobQualification, "Qualifications" } 
        };
        private readonly IJobOrganizationService _jobOrganizationService;
        private readonly IJobBasicService _jobBasicService;
        private readonly IJobDetailsService _jobDetailsService;
        private readonly IJobProgressService _jobProgressService;
        private readonly IJobActivateService _jobActivateService;

        public JobPostController(IJobOrganizationService jobOrganizationService, IJobBasicService jobBasicService, 
                                 IJobDetailsService jobDetailsService, IJobProgressService jobProgressService, IJobActivateService jobActivateService)
        {
            _jobOrganizationService = jobOrganizationService;
            _jobBasicService = jobBasicService;
            _jobDetailsService = jobDetailsService;
            _jobProgressService = jobProgressService;
            _jobActivateService = jobActivateService;
        }

        /*************************************************** Page 1 : Create Employer Account ***********************************************************/

        [HttpGet]
        public async Task<IActionResult> Retrieve(string? jobUid)
        {
            // Initialize DTO
            JobOrganizationDTO dto = new JobOrganizationDTO();

            // Get RefNo from Claims
            var refNo = User.FindFirstValue("RefNo");

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("To post a job, first create your employer profile."))
                            .WithErrors(this, ErrorError.GetErrors());

            int lastPage = 1;
            string? nextAction = null;

            // Only fetch the latest draft if jobUid is null
            if (string.IsNullOrEmpty(jobUid))
                jobUid = await _jobOrganizationService.GetLatestDraftsJobUidAsync(refNo);

            if (!string.IsNullOrEmpty(jobUid))
            {
                dto = await _jobOrganizationService.GetOrganizationDTOAsync(jobUid, refNo) ?? new JobOrganizationDTO();

                // Check last saved page
                lastPage = await _jobProgressService.GetLastSavedPageAsync(jobUid, refNo);

                bool isReturningToReview = HttpContext.Request.Query.ContainsKey("returnToReview");

                // Show SweetAlert only if a draft exists and user is not returning from Review
                if (!isReturningToReview && lastPage > 1)
                {
                    nextAction = PageActionMap.ContainsKey(lastPage) ? PageActionMap[lastPage] : "Retrieve";
                    TempData["LastPage"] = lastPage;
                    TempData["NextPageAction"] = nextAction;
                    TempData["JobUid"] = jobUid; // JS reads this to show SweetAlert
                }
            }

            return View("EmployerAccount", dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateOrganization(JobOrganizationDTO dto, string? jobUid, bool returnToReview = false)
        {
            ErrorError.Clear();

            var refNo = User.FindFirstValue("RefNo");
                if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                            .WithErrors(this, ErrorError.GetErrors());
        
         // # Check for jobUid.
            if (returnToReview && string.IsNullOrEmpty(jobUid))
                return RedirectToAction(nameof(Retrieve))
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

         // # Save the organization via service
         // # If saving failed, redirect back to the same page with errors
            var result = await _jobOrganizationService.SaveOrganizationAsync(dto, refNo, jobUid);
            if (!result)
                return RedirectToAction(nameof(Retrieve)).WithErrors(this, ErrorError.GetErrors());

            if (string.IsNullOrEmpty(jobUid))
                jobUid = Universal.Get<string>("JobUid");
            
            return !returnToReview ? 
                RedirectToAction("JobBasics", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess())
                        : RedirectToAction("Review", "JobReview", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }



        /*************************************************** Page 2 : Add Job Bacics ***********************************************************/

        [HttpGet]
        public async Task<IActionResult> JobBasics(string? jobUid)
        {
         // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction(nameof(Retrieve))
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

         // # To check the ownership by refNo to secure our query string url.
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                           .WithErrors(this, ErrorError.GetErrors());

         // # Fetch parent JobOrganization to validate ownership ( For safe query strings. (even if user provoked the query string we will get our exact same jobuid )).
            var jobOrg = await _jobOrganizationService.GetByJobUidAsync(jobUid);
            if (jobOrg == null || jobOrg.RefNo != refNo)
                return RedirectToAction(nameof(Retrieve))
                       .With(() => ErrorError.SetError("You do not have permission to access this job."))
                       .WithErrors(this, ErrorError.GetErrors());

            var dto = await _jobBasicService.GetJobBasicsDTOAsync(jobUid) ?? new JobBasicDTO { JobUid = jobUid };

            ViewBag.States = LocationHelper.States;
            ViewBag.Cities = LocationHelper.Cities;
            ViewBag.Areas  = LocationHelper.Areas;

            return View("JobBasics", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> updateJobBaics(JobBasicDTO dto, string jobUid, bool returnToReview = false)
        {
            ErrorError.Clear();

         // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction(nameof(Retrieve))
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

            var result = await _jobBasicService.SaveJobBasicsAsync(dto, jobUid);

            if (!result)
                return RedirectToAction(nameof(JobBasics), new { jobUid }).WithErrors(this, ErrorError.GetErrors());

            return !returnToReview ? 
                RedirectToAction("JobDetails", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess())
                        : RedirectToAction("Review", "JobReview", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }



        /*************************************************** Page 3 : Add Job Details ***********************************************************/

        [HttpGet]
        public async Task<IActionResult> JobDetails(string? jobUid)
        {
         // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction(nameof(Retrieve))
                    .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                    .WithErrors(this, ErrorError.GetErrors());

         // # To check the ownership by refNo to secure our query string url.
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                       .WithErrors(this, ErrorError.GetErrors());

         // # Fetch parent JobOrganization to validate ownership ( For safe query strings. (even if user provoked the query string we will get our exact same jobuid )).
            var jobOrg = await _jobOrganizationService.GetByJobUidAsync(jobUid);
            if (jobOrg == null || jobOrg.RefNo != refNo)
                return RedirectToAction(nameof(Retrieve))
                       .With(() => ErrorError.SetError("You do not have permission to access this job."))
                       .WithErrors(this, ErrorError.GetErrors());

            var dto = await _jobDetailsService.GetJobDetailsDTOAsync(jobUid) ?? new JobDetailDTO { JobUid = jobUid };

            ViewBag.EmploymentType = EmploymentTypes.EmploymentType;
            ViewBag.NumberOfPeopleToHire = EmploymentTypes.NumberOfPeopleToHire;

            return View("JobDetails", dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateJobDeatils(JobDetailDTO dto, string jobUid, bool returnToReview = false)
        {
            ErrorError.Clear();

         // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction(nameof(Retrieve))
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

         // # Create/Update
            var result = await _jobDetailsService.SaveJobDetailsAsync(dto, jobUid);
            if (!result)
                return RedirectToAction(nameof(JobDetails), new { jobUid }) .WithErrors(this, ErrorError.GetErrors());

            return !returnToReview
                 ? RedirectToAction("PayBenefits", "JobDetails", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess())
                     : RedirectToAction("Review", "JobReview", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }



        /*************************************************** Soft Delete the Draft (when user click start over SweetAlert Resume Draft) ***********************************************************/

     // # system-managed soft delete ( Prompts via SweetAlert to either resume from the last saved page or start over. )
        public async Task<IActionResult> SoftDeleteDraft([FromBody] string jobUid)
        {
            if (string.IsNullOrEmpty(jobUid))
                return Json(new { success = true }); 

            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return Json(new { success = true }); 

            await _jobActivateService.SoftDeleteDraftAsync(jobUid, refNo);

            return Json(new { success = true });
        }

    }
}



//var refNo = "83K3-ERJ2-9CHJ-SNGJ-GQ9C-49IX";
