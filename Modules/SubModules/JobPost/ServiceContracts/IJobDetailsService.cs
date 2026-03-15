using IndeedClone.Modules.SubModules.JobPost.DTO;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobDetailsService
    {
        public Task<bool> SaveJobDetailsAsync(JobDetailDTO dto, string jobUid);
        public Task<JobDetailDTO?> GetJobDetailsDTOAsync(string jobUid);
        public Task<int?> GetCurrentPageAsync(string jobUid);
    }
}
