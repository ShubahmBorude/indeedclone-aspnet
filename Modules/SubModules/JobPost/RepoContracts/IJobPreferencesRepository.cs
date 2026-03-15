using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.RepoContracts
{
    public interface IJobPreferencesRepository
    {
        Task<JobPreferencesModel> GetByJobUidAsync(string jobUid);
        Task<int?> GetCurrentPageAsync(string jobUid);
        Task CreateAsync(JobPreferencesModel entity);
        Task UpdateAsync(JobPreferencesModel entity);
    }
}
