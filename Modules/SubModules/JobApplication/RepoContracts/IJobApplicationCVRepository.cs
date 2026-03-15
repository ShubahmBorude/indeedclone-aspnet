using IndeedClone.Modules.SubModules.JobApplication.Models;

namespace IndeedClone.Modules.SubModules.JobApplication.RepoContracts
{
    public interface IJobApplicationCVRepository
    {
        Task<JobApplicationCVModel?> GetByApplicationUidAsync(string applicationUid);

        Task CreateAsync(JobApplicationCVModel entity);

        Task UpdateAsync(JobApplicationCVModel entity);

        Task SoftDeleteAsync(string applicationUid);

    }
}
