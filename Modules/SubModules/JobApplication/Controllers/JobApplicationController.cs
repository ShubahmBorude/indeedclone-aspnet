using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.SubModules.JobApplication.DTO;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Locations;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace IndeedClone.Modules.SubModules.JobApplication.Controllers
{
    public class JobApplicationController : Controller
    {
        private static readonly Dictionary<int, string> PageActionMap = new()
        {
            { (int)JobApplicationPages.CV, "CV" }, { (int)JobApplicationPages.Experience, "RelevantExperience" }, 
            { (int)JobApplicationPages.ScreenerQuestions, "ScreenerQuestions" },{ (int)JobApplicationPages.JobApplicationReview, "Review" }
        };
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IRelExperienceService _relExperienceService;
        private readonly IScreenerQuestionsService _screenerQuestionsService;
        private readonly IJobApplicationReviewService _jobApplicationReviewService;
        private readonly IJobApplicationActivateService _jobApplicationActivateService;
        private readonly IJobApplicationProgressService _jobApplicationProgressService;
        public JobApplicationController(IJobApplicationService jobApplicationService, IRelExperienceService relExperienceService, IScreenerQuestionsService screenerQuestionsService,
                                        IJobApplicationReviewService jobApplicationReviewService, IJobApplicationActivateService jobApplicationActivateService, IJobApplicationProgressService jobApplicationProgressService)
        {
            _jobApplicationService = jobApplicationService;
            _relExperienceService = relExperienceService;
            _screenerQuestionsService = screenerQuestionsService;
            _jobApplicationReviewService = jobApplicationReviewService;
            _jobApplicationActivateService = jobApplicationActivateService;
            _jobApplicationProgressService = jobApplicationProgressService;
        }


        /*************************************************** Method to handle Apply button in Background   ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> Apply(string? jobUid)
        {
            ErrorError.Clear();

            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("To apply for a job, please create your account."))
                            .WithErrors(this, ErrorError.GetErrors());
        // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

            var applicationUid = await _jobApplicationService.HandleApplyAsync(refNo, jobUid!);

            if (!ErrorError.CheckError())
                return RedirectToAction("Retrieve", "IndeedClone").WithErrors(this, ErrorError.GetErrors());

            return RedirectToAction(nameof(CV), new { applicationUid });
        }


        /*************************************************** Page 1 : Upload CV ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> CV(string applicationUid)
        {
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("To apply for a job, please create your account."))
                            .WithErrors(this, ErrorError.GetErrors());

            if(string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                      .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                           .WithErrors(this, ErrorError.GetErrors());

            var dto = await _jobApplicationService.GetCVDTOAsync(refNo, applicationUid);
            if (!ErrorError.CheckError() || dto == null)
                return RedirectToAction("Retrieve", "IndeedClone").WithErrors(this, ErrorError.GetErrors());

            int lastPage = 1;
            string? nextAction = null;

            var application = await _jobApplicationService.GetByApplicationUid(applicationUid);

            if (application != null && application.Status == JobApplicationStatus.DRAFT)
            {
                lastPage = (int) await _jobApplicationProgressService.GetLastSavedPageAsync(applicationUid, refNo);
             // # Show SweetAlert only if a draft exists and user is not returning from Review
                bool isReturningToReview = HttpContext.Request.Query.ContainsKey("returnToReview");

                if (!isReturningToReview && lastPage > 1)
                {
                    nextAction = PageActionMap.ContainsKey(lastPage) ? PageActionMap[lastPage] : "CV";
                    TempData["LastPage"] = lastPage;
                    TempData["NextPageAction"] = nextAction;
                    TempData["ApplicationUid"] = applicationUid;
                }
            }

            return View("CV", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCV(string applicationUid, IFormFile file, bool returnToReview = false)
        {
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("To apply for a job, please create your account."))
                            .WithErrors(this, ErrorError.GetErrors());

            if (string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                            .WithErrors(this, ErrorError.GetErrors());


            var result = await _jobApplicationService.UploadCVAsync(refNo, applicationUid, file);

            if (!result)
            {
                var dto = await _jobApplicationService.GetCVDTOAsync(refNo, applicationUid);
                return RedirectToAction(nameof(CV), new { applicationUid }).WithErrors(this, ErrorError.GetErrors());
            }

            return !returnToReview
                 ? RedirectToAction("RelevantExperience", "JobApplication", new { applicationUid }).With(() => TempData["Success"] = ErrorError.GetSuccess())
                     : RedirectToAction("Review", "JobApplication", new { applicationUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }

      

        [HttpGet]
        public async Task<IActionResult> DownloadCV(string? applicationUid)
        {
            var refNo = User.FindFirstValue("RefNo");

            var result = await _jobApplicationService.DownloadCVAsync(refNo!, applicationUid!);

            if (!ErrorError.CheckError() || result == null)
                return RedirectToAction("Retrieve", "IndeedClone").WithErrors(this, ErrorError.GetErrors());

            return File(result.Value.fileBytes, "application/octet-stream", result.Value.fileName);
        }


        /*************************************************** Page 2 : Add Reevent Experience ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> RelevantExperience(string? applicationUid)
        {
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                            .WithErrors(this, ErrorError.GetErrors());

            if (string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                            .WithErrors(this, ErrorError.GetErrors());

            var oweneship = await _jobApplicationService.GetByApplicationUid(applicationUid);
            if (oweneship == null || oweneship.RefNo != refNo)
                return RedirectToAction(nameof(CV))
                       .With(() => ErrorError.SetError("Something went wrong. You cannot apply to this job."))
                            .WithErrors(this, ErrorError.GetErrors());

            var dto = await _relExperienceService.GetRelExperienceDTOAsync(applicationUid);

            return View("exp", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRelevenExperience(RelExperienceDTO dto, string applicationUid, bool returnToReview = false)
        {
            ErrorError.Clear();

            if (string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                            .WithErrors(this, ErrorError.GetErrors());

            var result = await _relExperienceService.SaveExperienceAsync(dto, applicationUid);
            if (!result)
                return RedirectToAction(nameof(RelevantExperience), new { applicationUid }).WithErrors(this, ErrorError.GetErrors());

            return !returnToReview
                 ? RedirectToAction("ScreenerQuestions", "JobApplication", new { applicationUid }).With(() => TempData["Success"] = ErrorError.GetSuccess())
                     : RedirectToAction("Review", "JobApplication", new { applicationUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());

        }


        /*************************************************** Page 3 : Screener EMployee Questions ***********************************************************/

        [HttpGet]
        public async Task<IActionResult> ScreenerQuestions(string? applicationUid)
        {
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                            .WithErrors(this, ErrorError.GetErrors());

            if (string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                            .WithErrors(this, ErrorError.GetErrors());

            var oweneship = await _jobApplicationService.GetByApplicationUid(applicationUid);
            if (oweneship == null || oweneship.RefNo != refNo)
                return RedirectToAction(nameof(CV))
                       .With(() => ErrorError.SetError("Something went wrong. You cannot apply to this job."))
                            .WithErrors(this, ErrorError.GetErrors());

            var dto = await _screenerQuestionsService.GetScreenerQuestionsDTOAsync(applicationUid);

            ViewBag.States = LocationHelper.States;
            ViewBag.Cities = LocationHelper.Cities;
            ViewBag.Areas = LocationHelper.Areas;
            ViewBag.Education = EmployeQualifications.Education;

            return View("empque", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateScreenerQuestions(ScreenerQuestionsDTO dto, string applicationUid, bool returnToReview = false)
        {
            ErrorError.Clear();

            if (string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                            .WithErrors(this, ErrorError.GetErrors());

            var result = await _screenerQuestionsService.SaveScreenerQuestionsAsync(dto, applicationUid);

            if (result != ScreenerQuestionsResult.Success)
            {
                if (result == ScreenerQuestionsResult.ExperienceMismatch)
                    return RedirectToAction("Retrieve", "IndeedClone") .WithErrors(this, ErrorError.GetErrors());

                return RedirectToAction(nameof(ScreenerQuestions), new { applicationUid }).WithErrors(this, ErrorError.GetErrors());
            }

            return RedirectToAction("Review", "JobApplication", new { applicationUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }


        /*************************************************** Page 4 : Review ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> Review(string? applicationUid)
        {
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                            .WithErrors(this, ErrorError.GetErrors());

            if (string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                            .WithErrors(this, ErrorError.GetErrors());

            var oweneship = await _jobApplicationService.GetByApplicationUid(applicationUid);
            if (oweneship == null || oweneship.RefNo != refNo)
                return RedirectToAction(nameof(CV))
                       .With(() => ErrorError.SetError("Something went wrong. You cannot apply to this job."))
                            .WithErrors(this, ErrorError.GetErrors());


            var dto = await _jobApplicationReviewService.GetJobApplicationReviewDTOAsync(applicationUid, refNo);
            ErrorError.SetSuccess("You apply to this job Successfully");

            return View("review", dto);
        }

        [HttpGet]
        public async Task<IActionResult> Submit(string applicationUid)
        {
            ErrorError.Clear();

            if (string.IsNullOrWhiteSpace(applicationUid))
                return RedirectToAction("Retrieve", "IndeedClone")
                       .With(() => ErrorError.SetError("Application identifier missing. Please apply again."))
                            .WithErrors(this, ErrorError.GetErrors());

            await _jobApplicationActivateService.SubmitAsync(applicationUid);
            if (!ErrorError.CheckError())
                return RedirectToAction(nameof(Review), new { applicationUid }).WithErrors(this, ErrorError.GetErrors());

         // # This is Success
            return RedirectToAction("Retrieve", "IndeedClone").With(() => TempData["Success"] = ErrorError.GetSuccess());

        }

        /*************************************************** Soft Delete the Draft (when user click start over SweetAlert Resume Draft) ***********************************************************/


     // # system-managed soft delete ( Prompts via SweetAlert to either resume from the last saved page or start over. )
        public async Task<IActionResult> SoftDeleteDraft([FromBody] string? applicationUid)
        {
            if (string.IsNullOrEmpty(applicationUid))
                return Json(new { success = true });

            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return Json(new { success = true });

            await _jobApplicationActivateService.SoftDeleteDraftAsync(applicationUid, refNo);

            return Json(new { success = true });
        }
    }
}
