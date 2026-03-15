using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.RepoContracts
{
    public interface IJobOrganizationRepository
    {
        Task<JobOrganizationModel> GetByJobUidAsync(string JobUid);
        Task<string?> GetLatestDraftJobUidAsync(string refNo);
        Task<int?> GetCurrentPageAsync(string jobUid);
        Task CreateAsync(JobOrganizationModel entity);
        Task UpdateAsync(JobOrganizationModel entity);
        Task SaveEditedAsync(string jobUid);

    }
}
