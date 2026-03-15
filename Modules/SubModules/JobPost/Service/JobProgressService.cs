using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;

namespace IndeedClone.Modules.SubModules.JobPost.Service
{
    public class JobProgressService : IJobProgressService
    {
        private readonly IJobOrganizationService _jobOrganizationService;
        private readonly IJobBasicService _jobBasicService;
        private readonly IJobDetailsService _jobDetailsService;
        private readonly IJobPayBenefitsService _jobPayBenefitsService;
        private readonly IJobDescriptionService _jobDescriptionService;
        private readonly IJobPreferencesService _jobPreferencesService;
        private readonly IJobQualificationsService _jobQualificationsService;

        public JobProgressService(IJobOrganizationService jobOrganizationService, IJobBasicService jobBasicService, IJobDetailsService jobDetailsService,
                                 IJobPayBenefitsService jobPayBenefitsService, IJobDescriptionService jobDescriptionService, IJobPreferencesService jobPreferencesService,
                                 IJobQualificationsService jobQualificationsService)
        {
            _jobOrganizationService = jobOrganizationService;
            _jobBasicService = jobBasicService;
            _jobDetailsService = jobDetailsService;
            _jobPayBenefitsService = jobPayBenefitsService;
            _jobDescriptionService = jobDescriptionService;
            _jobPreferencesService = jobPreferencesService;
            _jobQualificationsService = jobQualificationsService;
        }

        public async Task<int> GetLastSavedPageAsync(string jobUid, string refNo)
        { // Default start page
            int lastPage = 1;

            // Fetch all pages
            var pages = new int?[]
            {
                await GetJobOrganizationPageAsync(jobUid, refNo),
                await GetJobBasicsPageAsync(jobUid),
                await GetJobDetailsPageAsync(jobUid),
                await GetJobPayBenefitsPageAsync(jobUid),
                await GetJobPreferencesPageAsync(jobUid),
                await GetJobDescriptionPageAsync(jobUid),
                await GetJobQualificationsPageAsync(jobUid)
            };

            foreach (var page in pages)
            {
                if (page.HasValue && page.Value >= lastPage)
                    lastPage = page.Value + 1; // <-- key fix: move to next page
            }

            // Safety: max page 8
            if (lastPage > 8) lastPage = 8;

            return lastPage;
        }

        /***************************************** Private SRP Methods ****************************************************/

        private async Task<int?> GetJobOrganizationPageAsync(string jobUid, string refNo)
        {
            return await _jobOrganizationService.GetCurrentPageAsync(jobUid);
        }

        private async Task<int?> GetJobBasicsPageAsync(string jobUid)
        {
            return await _jobBasicService.GetCurrentPageAsync(jobUid); 
        }

        private async Task<int?> GetJobDetailsPageAsync(string jobUid)
        {
            return await _jobDetailsService.GetCurrentPageAsync(jobUid);
        }

        private async Task<int?> GetJobPayBenefitsPageAsync(string jobUid)
        {
            return await _jobPayBenefitsService.GetCurrentPageAsync(jobUid);
        }

        private async Task<int?> GetJobPreferencesPageAsync(string jobUid)
        {
            return await _jobPreferencesService.GetCurrentPageAsync(jobUid);
        }

        private async Task<int?> GetJobDescriptionPageAsync(string jobUid)
        {
            return await _jobDescriptionService.GetCurrentPageAsync(jobUid);
        }

        private async Task<int?> GetJobQualificationsPageAsync(string jobUid)
        {
            return await _jobQualificationsService.GetCurrentPageAsync(jobUid);
        }

    }
}
