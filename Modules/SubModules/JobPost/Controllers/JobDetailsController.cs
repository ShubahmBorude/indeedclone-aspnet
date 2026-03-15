using Humanizer;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IndeedClone.Modules.SubModules.JobPost.Controllers
{
    public class JobDetailsController : Controller
    {
        private readonly IJobOrganizationService _jobOrganizationService;
        private readonly IJobPayBenefitsService _jobPayBenefitsService;
        private readonly IJobDescriptionService _jobDescriptionService;
        private readonly IJobPreferencesService _jobPreferencesService;

        public JobDetailsController(IJobOrganizationService jobOrganizationService, IJobPayBenefitsService jobPayBenefitsService, 
                                        IJobDescriptionService jobDescriptionService, IJobPreferencesService jobPreferencesService)
        {
            _jobOrganizationService = jobOrganizationService;
            _jobPayBenefitsService = jobPayBenefitsService;
            _jobDescriptionService = jobDescriptionService;
            _jobPreferencesService = jobPreferencesService;
        }


        /*************************************************** Page 4 : Add Payment & Benefits ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> PayBenefits(string? jobUid)
        {
         // # Check for jobUid
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "JobPost")
                    .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                        .WithErrors(this, ErrorError.GetErrors());

         // # Check logged-in user refNo
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                    .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                        .WithErrors(this, ErrorError.GetErrors());

         // # Ownership validation via JobOrganization
            var jobOrg = await _jobOrganizationService.GetByJobUidAsync(jobUid);
            if (jobOrg == null || jobOrg.RefNo != refNo)
                return RedirectToAction("Retrieve", "JobPost")
                    .With(() => ErrorError.SetError("You do not have permission to access this job."))
                        .WithErrors(this, ErrorError.GetErrors());

            var dto = await _jobPayBenefitsService.GetPayBenefitsDTOAsync(jobUid) ?? new JobPayBenefitsDTO { JobUid = jobUid };

            ViewBag.EmployeeSupplementedBenefits = EmploymentTypes.EmployeeSupplementedBenefits;
            ViewBag.Benefits = EmploymentTypes.Benefits;

            return View("EmployePaymentBenifits", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePayBenefits(JobPayBenefitsDTO dto, string jobUid, bool returnToReview = false)
        {
            ErrorError.Clear();

         // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "JobPost")
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

            var result = await _jobPayBenefitsService.SavePayBenefitsAsync(dto, jobUid);
            if (!result)
                return RedirectToAction("PayBenefits", "JobDetails", new { jobUid }).WithErrors(this, ErrorError.GetErrors());

            return !returnToReview ?
                  RedirectToAction("JobDescription", "JobDetails", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess())
                        : RedirectToAction("Review", "JobReview", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }



        /*************************************************** Page 5 : Add Job Description ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> JobDescription(string? jobUid)
        {
         // # Check for jobUid
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "JobPost")
                    .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                        .WithErrors(this, ErrorError.GetErrors());

         // # Check logged-in user refNo
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                    .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                        .WithErrors(this, ErrorError.GetErrors());

         // # Ownership validation via JobOrganization
            var jobOrg = await _jobOrganizationService.GetByJobUidAsync(jobUid);
            if (jobOrg == null || jobOrg.RefNo != refNo)
                return RedirectToAction("Retrieve", "JobPost")
                    .With(() => ErrorError.SetError("You do not have permission to access this job."))
                        .WithErrors(this, ErrorError.GetErrors());

            var dto = await _jobDescriptionService.GetJobDescriptionDTOAsync(jobUid) ?? new JobDescriptionDTO { JobUid = jobUid };

            return View("JobDescription", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateJobDescription(JobDescriptionDTO dto, string jobUid, bool returnToReview = false)
        {
            ErrorError.Clear();

         // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "JobPost")
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

            var result = await _jobDescriptionService.SaveJobDescriptionAsync(dto, jobUid);
            if (!result)
                return RedirectToAction(nameof(JobDescription), new { jobUid }).WithErrors(this, ErrorError.GetErrors());

            return !returnToReview ?
                  RedirectToAction("JobPreferences", "JobDetails", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess())
                        : RedirectToAction("Review", "JobReview", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }



        /*************************************************** Page 6 : Add Job Preferences ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> JobPreferences(string? jobUid)
        {
         // # Check for jobUid
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "JobPost")
                    .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                        .WithErrors(this, ErrorError.GetErrors());

         // # Check logged-in user refNo
            var refNo = User.FindFirstValue("RefNo");
            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Retrieve", "IndeedClone")
                    .With(() => ErrorError.SetError("Unable to determine your account. Please log in again."))
                        .WithErrors(this, ErrorError.GetErrors());

         // # Ownership validation via JobOrganization
            var jobOrg = await _jobOrganizationService.GetByJobUidAsync(jobUid);
            if (jobOrg == null || jobOrg.RefNo != refNo)
                return RedirectToAction("Retrieve", "JobPost")
                    .With(() => ErrorError.SetError("You do not have permission to access this job."))
                        .WithErrors(this, ErrorError.GetErrors());

            var dto = await _jobPreferencesService.GetJobPreferencesDTOAsync(jobUid) ?? new JobPreferencesDTO { JobUid = jobUid };

            return View("JobPreferences", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateJobPreferences(JobPreferencesDTO dto, string jobUid)
        {
            ErrorError.Clear();

         // # Check for jobUid.
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "JobPost")
                      .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                           .WithErrors(this, ErrorError.GetErrors());

            var result = await _jobPreferencesService.SavePreferencesAsync(dto, jobUid);
            if (!result)
                return RedirectToAction(nameof(JobPreferences), new { jobUid }).WithErrors(this, ErrorError.GetErrors());

            return RedirectToAction("Review", "JobReview", new { jobUid }).With(() => TempData["Success"] = ErrorError.GetSuccess());
        }
    }
}
