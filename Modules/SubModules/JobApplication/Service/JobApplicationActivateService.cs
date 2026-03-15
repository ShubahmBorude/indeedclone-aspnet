using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobApplication.Service
{
    public class JobApplicationActivateService : IJobApplicationActivateService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
     
        public JobApplicationActivateService(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

     // # Submit Final Application
        public async Task SubmitAsync(string applicationUid)
        {
            ErrorError.Clear();

            var application = await _jobApplicationRepository.GetByApplicationUidAsync(applicationUid);

            if (application == null)
            {
                ErrorError.SetError("Application not found.");
                return;
            }
            if (application.Status == JobApplicationStatus.SUBMITTED)
            {
                ErrorError.SetError("Application already submitted.");
                return;
            }

            if (application.CurrentPage < JobApplicationPages.ScreenerQuestions)
            {
                ErrorError.SetError("Please complete all steps before submitting.");
                return;
            }

            await _jobApplicationRepository.UpdateStatusAsync(applicationUid, JobApplicationStatus.SUBMITTED);

            ErrorError.SetSuccess("Application submitted successfully.");
        }

        public async Task SoftDeleteDraftAsync(string applicationUid, string refNo)
        {
            if (string.IsNullOrWhiteSpace(applicationUid) || string.IsNullOrWhiteSpace(refNo))
                return;

            var application = await _jobApplicationRepository.GetByApplicationUidRefNoAsync(applicationUid, refNo);

            if (application == null)
                return;

         // # Only DRAFT can be deleted by cron (Sweetalert)
            if (application.Status != JobApplicationStatus.DRAFT)
                return;

            await _jobApplicationRepository.SoftDeleteJobApplicationAsync(applicationUid, refNo);
        }
    }
}
