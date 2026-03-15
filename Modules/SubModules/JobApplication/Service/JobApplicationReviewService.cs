using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobApplication.DTO;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;

namespace IndeedClone.Modules.SubModules.JobApplication.Service
{
    public class JobApplicationReviewService : IJobApplicationReviewService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly IRelExperienceService _relExperienceService;
        private readonly IScreenerQuestionsService _screenerQuestionsService;

        public JobApplicationReviewService(IJobApplicationRepository jobApplicationRepository, IJobApplicationService jobApplicationService, 
                                           IRelExperienceService relExperienceService, IScreenerQuestionsService screenerQuestionsService)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _jobApplicationService = jobApplicationService;
            _relExperienceService = relExperienceService;
            _screenerQuestionsService = screenerQuestionsService;
        }

        public async Task<JobApplicationReviewDTO?> GetJobApplicationReviewDTOAsync(string applicationUid, string refNo)
        {
            return new JobApplicationReviewDTO
            {
                CV = await _jobApplicationService.GetCVDTOAsync(refNo, applicationUid),
                RelExperience = await _relExperienceService.GetRelExperienceDTOAsync(applicationUid),
                ScreenerQuestions = await _screenerQuestionsService.GetScreenerQuestionsDTOAsync(applicationUid),
            };
 
        }

    }
}
