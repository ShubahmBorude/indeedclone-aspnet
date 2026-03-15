using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;


namespace IndeedClone.Modules.SubModules.JobApplication.Service
{
    public class JobApplicationProgressService : IJobApplicationProgressService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationProgressService(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<JobApplicationPages?> GetLastSavedPageAsync(string applicationUid, string refNo)
        {
            var application = await _jobApplicationRepository.GetByApplicationUidAsync(applicationUid);

            if (application == null || application.RefNo != refNo)
                return null;

            var nextPageValue = (int)application.CurrentPage + 1;

         // # don't exceed max enum
            if (nextPageValue > (int)JobApplicationPages.JobApplicationReview)
                nextPageValue = (int)JobApplicationPages.JobApplicationReview;

            return (JobApplicationPages)nextPageValue;
        }
    }

       
}

