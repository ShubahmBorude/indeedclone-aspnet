using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Models;

namespace IndeedClone.Modules.SubModules.JobApplication.RepoContracts
{
    public interface IJobApplicationRepository
    {
        Task<JobApplicationModel?> GetByUserAndJobAsync(string refNo, string jobUid);

        Task<JobApplicationModel?> GetByApplicationUidAsync(string applicationUid);

        Task<JobApplicationModel?> GetByApplicationUidRefNoAsync(string applicationUid, string refNo);

        Task CreateDraftAsync(JobApplicationModel model);

        Task UpdateProgressAsync(string applicationUid, JobApplicationPages currentPage);

        Task UpdateStatusAsync(string applicationUid, JobApplicationStatus status);

        Task SoftDeleteJobApplicationAsync(string applicationUid, string refNo);
    }
}
