using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.RepoContracts
{
    public interface IJobQualificationsRepository
    {
        Task<JobQualificationsModel> GetByJobUidAsync(string jobUid);
        Task<int?> GetCurrentPageAsync(string jobUid);

        Task CreateAsync(JobQualificationsModel entity);
        Task UpdateAsync(JobQualificationsModel entity);

    }
}
