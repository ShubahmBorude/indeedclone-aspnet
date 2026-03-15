using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.RepoContracts
{
    public interface IJobDescriptionRepository
    {
        Task<JobDescriptionModel> GetByJobUidAsync(string jobUid);
        Task<int?> GetCurrentPageAsync(string jobUid);
        Task CreateAsync(JobDescriptionModel entity);
        Task UpdateAsync(JobDescriptionModel entity);
    }
}
