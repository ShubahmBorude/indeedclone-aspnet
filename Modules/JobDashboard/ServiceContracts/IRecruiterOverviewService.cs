using IndeedClone.Modules.JobDashboard.DTO;

namespace IndeedClone.Modules.JobDashboard.ServiceContracts
{
    public interface IRecruiterOverviewService
    {
     // # Method to get Recruiter data and actions
        Task<RecruiterOverviewPaginatedDTO?> GetRecruiterOverviewAsync(string refNo, int page, int pageSize);

     // # Method to get Applicant list
        Task<ApplicantListPaginatedDTO?> GetApplicantsByJobAsync(string jobUid, int page, int pageSize, string refNo);
        Task<bool> IsJobOwnedByRecruiterAsync(string jobUid, string refNo);

     // # Method to update the job Application status by recruiter action
        Task<bool> UpdateApplicationStatusAsync(string applicationUid, string newStatus);

     // # Method to Get the complte details of candidate
        Task<CandidateDetailsDTO?> GetCandidateDetailsAsync(string applicationUid, string recruiterRefNo);

     // # Method to get preview of cv
        Task<byte[]?> PreviewCVAsync(string applicationUid);

     // # Method to download the cv
        Task<(byte[] fileBytes, string fileName)?> DownloadCVAsync(string applicationUid);

    }
}

