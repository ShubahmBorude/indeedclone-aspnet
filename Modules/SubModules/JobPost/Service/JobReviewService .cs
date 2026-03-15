using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;

namespace IndeedClone.Modules.SubModules.JobPost.Service
{
    public class JobReviewService : IJobReviewService
    {
        private readonly IJobOrganizationService _jobOrganizationService;
        private readonly IJobBasicService _jobBasicService;
        private readonly IJobDetailsService _jobDetailsService;
        private readonly IJobPayBenefitsService _jobPayBenefitsService;
        private readonly IJobDescriptionService _jobDescriptionService;
        private readonly IJobPreferencesService _jobPreferencesService;

        public JobReviewService(IJobOrganizationService jobOrganizationService, IJobBasicService jobBasicService, IJobDetailsService jobDetailsService, 
                                IJobPayBenefitsService jobPayBenefitsService, IJobDescriptionService jobDescriptionService, IJobPreferencesService jobPreferencesService)
        {
            _jobOrganizationService = jobOrganizationService;
            _jobBasicService = jobBasicService;
            _jobDetailsService = jobDetailsService;
            _jobPayBenefitsService = jobPayBenefitsService;
            _jobDescriptionService = jobDescriptionService;
            _jobPreferencesService = jobPreferencesService;
        }

        public async Task<JobReviewDTO?> GetJobReviewAsync(string jobUid, string refNo)
        {
            return new JobReviewDTO 
            {
                JobOrganization = await _jobOrganizationService.GetOrganizationDTOAsync(jobUid, refNo),
                JobBasic = await _jobBasicService.GetJobBasicsDTOAsync(jobUid),
                JobDetail = await _jobDetailsService.GetJobDetailsDTOAsync(jobUid),
                PayBenefits = await _jobPayBenefitsService.GetPayBenefitsDTOAsync(jobUid),
                JobDescription = await _jobDescriptionService.GetJobDescriptionDTOAsync(jobUid),
                JobPreferences = await _jobPreferencesService.GetJobPreferencesDTOAsync(jobUid)
            };
        }
    }
}
