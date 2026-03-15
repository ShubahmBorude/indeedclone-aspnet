using IndeedClone.Modules.SubModules.JobPost.DTO;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobPreferencesService
    {
        public Task<bool> SavePreferencesAsync(JobPreferencesDTO dto, string jobUid);
        public Task<JobPreferencesDTO?> GetJobPreferencesDTOAsync(string jobUid);
        public Task<int?> GetCurrentPageAsync(string jobUid);
    }
}
