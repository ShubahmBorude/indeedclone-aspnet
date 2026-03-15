using IndeedClone.Modules.SubModules.JobPost.DTO;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobReviewService
    {
        Task<JobReviewDTO?> GetJobReviewAsync(string jobUid, string refNo);
    }
}
