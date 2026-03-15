using IndeedClone.Modules.SubModules.JobPost.DTO;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobBasicService
    {
        public Task<bool> SaveJobBasicsAsync(JobBasicDTO dto, string jobUid);
        public Task<JobBasicDTO?> GetJobBasicsDTOAsync(string jobUid);
        public Task<int?> GetCurrentPageAsync(string jobUid);
    }
}
