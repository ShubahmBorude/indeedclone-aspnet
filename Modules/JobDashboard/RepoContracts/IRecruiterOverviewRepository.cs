using IndeedClone.Modules.JobDashboard.DTO;
using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.JobDashboard.RepoContracts
{
    public interface IRecruiterOverviewRepository
    {
        Task<RecruiterOverviewPaginatedDTO?> GetRecruiterJobsOverviewAsync(string refNo, int page, int pageSize);
        Task<ApplicantListPaginatedDTO?> GetApplicantsByJobAsync(string jobUid, int page, int pageSize);
        Task<bool> IsJobOwnedByRecruiterAsync(string jobUid, string refNo);
        Task<bool> UpdateApplicationStatusAsync(string applicationUid, JobApplicationStatus newStatus);
        Task<CandidateDetailsDTO?> GetCandidateDetailsAsync(string applicationUid, string refNo);
    }
}
