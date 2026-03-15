using IndeedClone.Modules.JobDashboard.DTO;
using IndeedClone.Modules.JobDashboard.ServiceContracts;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace IndeedClone.Modules.JobDashboard.Controllers
{
    public class JobDashboardController : Controller
    {
        private readonly IRecruiterOverviewService _recruiterOverviewService;

        public JobDashboardController(IRecruiterOverviewService recruiterOverviewService)
        {
            _recruiterOverviewService = recruiterOverviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Retrieve(int page = 1, int pageSize = 5)
        {
            ErrorError.Clear();

         // # Get RefNo from Claims
            var refNo = User.FindFirstValue("RefNo");

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("To acess Job Dashboard, first create your account."))
                            .WithErrors(this, ErrorError.GetErrors());

            var result = await _recruiterOverviewService.GetRecruiterOverviewAsync(refNo, page, pageSize);

            return View("recruiterOverview", result);
        }

        [HttpGet]
        public async Task<IActionResult> Applicants(string jobUid, int page = 1, int pageSize = 10)
        {
            ErrorError.Clear();

         // # Get RefNo from Claims
            var refNo = User.FindFirstValue("RefNo");

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("To acess Job Dashboard, first create your account."))
                            .WithErrors(this, ErrorError.GetErrors());

            var isOwned = await _recruiterOverviewService.IsJobOwnedByRecruiterAsync(jobUid, refNo);
            if (!isOwned)
                return RedirectToAction("Retrieve", "JobDashboard")
                       .With(() => ErrorError.SetError("You do not have access to this job's applicants."))
                            .WithErrors(this, ErrorError.GetErrors());

            var applicantList = await _recruiterOverviewService.GetApplicantsByJobAsync(jobUid, page, pageSize, refNo);

            return View("applicantList", applicantList);
        }

        [HttpGet]
        public async Task<IActionResult> CandidateDetails(string jobUid, string applicationUid)
        {
            ErrorError.Clear();

         // # Get RefNo from Claims
            var refNo = User.FindFirstValue("RefNo");

            if (string.IsNullOrEmpty(refNo))
                return RedirectToAction("Login", "Auth")
                       .With(() => ErrorError.SetError("To acess Job Dashboard, first create your account."))
                            .WithErrors(this, ErrorError.GetErrors());

            var dto = await _recruiterOverviewService.GetCandidateDetailsAsync(applicationUid, refNo);
            if (dto == null)
                return RedirectToAction(nameof(Applicants), new { jobUid })
                       .With(() => ErrorError.SetError("Something went wrong! could not fetch the candidate details."))
                            .WithErrors(this, ErrorError.GetErrors());

            return View("candidateDetails", dto);
        }

        [HttpGet]
        public async Task<IActionResult> PreviewCV(string? applicationUid)
        {
            ErrorError.Clear();

            var fileBytes = await _recruiterOverviewService.PreviewCVAsync(applicationUid!);

            if (!ErrorError.CheckError() || fileBytes == null)
                return NotFound();

            return File(fileBytes, "application/pdf");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadCV(string? applicationUid)
        {
            ErrorError.Clear();

            var result = await _recruiterOverviewService.DownloadCVAsync(applicationUid!);

            if (!ErrorError.CheckError() || result == null)
                return RedirectToAction(nameof(CandidateDetails), new { applicationUid }).WithErrors(this, ErrorError.GetErrors());

            return File(result.Value.fileBytes, "application/octet-stream", result.Value.fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateApplicantStatusDTO request)
        {
            ErrorError.Clear();

            if (request == null || string.IsNullOrEmpty(request.ApplicationUid))
                return BadRequest(new { success = false, message = "Invalid request." });

            var updated = await _recruiterOverviewService.UpdateApplicationStatusAsync(request.ApplicationUid, request.NewStatus);

            if (!updated)
                return NotFound(new { success = false, message = "Application not found or invalid status." });

            return Ok(new
            {
                success = true,
                message = $"Status updated to {request.NewStatus}"
            });
        }


    }

}
