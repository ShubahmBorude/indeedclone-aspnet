using IndeedClone.Modules.SubModules.JobPost.DTO;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobDescriptionService
    {
        public Task<bool> SaveJobDescriptionAsync(JobDescriptionDTO dto, string jobUid);
        public Task<JobDescriptionDTO?> GetJobDescriptionDTOAsync(string jobUid);
        public Task<int?> GetCurrentPageAsync(string jobUid);
    }
}
