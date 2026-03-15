using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IndeedClone.Modules.SubModules.JobPost.Controllers
{
    public class JobReviewController : Controller
    {
        private readonly IJobOrganizationService _jobOrganizationService;
        private readonly IJobBasicService _jobBasicService;
        private readonly IJobDetailsService _jobDetailsService;
        private readonly IJobPayBenefitsService _jobPayBenefitsService;
        private readonly IJobDescriptionService _jobDescriptionService;
        private readonly IJobPreferencesService _jobPreferencesService;
        private readonly IJobReviewService _jobReviewService;
        private readonly IJobQualificationsService _jobQualificationsService;
        private readonly IJobActivateService _jobActivateService;

        public JobReviewController(IJobOrganizationService jobOrganizationService, IJobBasicService jobBasicService, IJobDetailsService jobDetailsService, 
                                   IJobPayBenefitsService jobPayBenefitsService, IJobDescriptionService jobDescriptionService, IJobPreferencesService jobPreferencesService, 
                                   IJobReviewService jobReviewService, IJobQualificationsService jobQualificationsService, IJobActivateService jobActivateService)
        {
            _jobOrganizationService = jobOrganizationService;
            _jobBasicService = jobBasicService;
            _jobDetailsService = jobDetailsService;
            _jobPayBenefitsService = jobPayBenefitsService;
            _jobDescriptionService = jobDescriptionService;
            _jobPreferencesService = jobPreferencesService;
            _jobReviewService = jobReviewService;
            _jobQualificationsService = jobQualificationsService;
            _jobActivateService = jobActivateService;
        }


        /*************************************************** Page 7 : Review you job ***********************************************************/

        [HttpGet]
        public async Task<IActionResult> Review(string? jobUid)
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

            var reviewDto = await _jobReviewService.GetJobReviewAsync(jobUid, refNo);

            return View("JobPostReview", reviewDto);
        }



        /*************************************************** Page 8 : Add Job Qualifications ***********************************************************/


        [HttpGet]
        public async Task<IActionResult> Qualifications(string? jobUid)
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

            var dto = await _jobQualificationsService.GetJobQualificationsDTOAsync(jobUid) ?? new JobQualificationsDTO { JobUid = jobUid };

            ViewBag.Skills = EmployeQualifications.Skills;
            ViewBag.Education = EmployeQualifications.Education;
            ViewBag.Certification = EmployeQualifications.Certification;
            ViewBag.Languages = EmployeQualifications.Languages;

            return View("JobQualification", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQualifications(JobQualificationsDTO dto, string jobUid)
        {
            ErrorError.Clear();

            foreach (var key in HttpContext.Request.Form.Keys)
            {
                var value = HttpContext.Request.Form[key];
                Console.WriteLine($"Form Key: {key} = {value}");
            }
            //return null;
            // # Check for jobUid
            if (string.IsNullOrEmpty(jobUid))
                return RedirectToAction("Retrieve", "JobPost")
                    .With(() => ErrorError.SetError("Job identifier missing. Please start again."))
                        .WithErrors(this, ErrorError.GetErrors());

            var result = await _jobQualificationsService.SaveJobQualificationsAsync(dto, jobUid);
            if (!result)
                return RedirectToAction("Qualifications", "JobReview", new { jobUid }).WithErrors(this, ErrorError.GetErrors());

        // # Activate the Job
            await _jobActivateService.ActivateJobPostAsync(jobUid);
            if (!ErrorError.CheckError())
                return RedirectToAction("Qualifications", "JobReview", new { jobUid }).WithErrors(this, ErrorError.GetErrors());

         // # This is Success
            return RedirectToAction("Retrieve", "IndeedClone").With(() => TempData["Success"] = "Your Job has been posted successfully.");
        }
    }
}
