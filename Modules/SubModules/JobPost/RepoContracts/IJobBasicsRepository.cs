using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.RepoContracts
{
    public interface IJobBasicsRepository
    {
        Task<JobBasicsModel> GetByJobUidAsync(string jobUid);
        Task<int?> GetCurrentPageAsync(string jobUid);
        Task CreateAsync(JobBasicsModel entity);
        Task UpdateAsync(JobBasicsModel entity);
    }
}
