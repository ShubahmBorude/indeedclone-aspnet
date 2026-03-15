using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.RepoContracts
{
    public interface IJobDetailsRepository
    {
        Task<JobDetailsModel> GetByJobUidAsync(string jobUid);
        Task<int?> GetCurrentPageAsync(string jobUid);
        Task CreateAsync(JobDetailsModel entity);
        Task UpdateAsync(JobDetailsModel entity);
    }
}
