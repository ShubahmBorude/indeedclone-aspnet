using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.RepoContracts
{
    public interface IJobPayBenefitsRepository
    {
        Task<JobPayBenefitsModel> GetByJobUidAsync(string jobUid);
        Task<int?> GetCurrentPageAsync(string jobUid);
        Task CreateAsync(JobPayBenefitsModel entity);
        Task UpdateAsync(JobPayBenefitsModel entity);
    }
}
