using IndeedClone.Modules.SubModules.JobApplication.DTO;
using System;

namespace IndeedClone.Modules.SubModules.JobApplication.ServiceContracts
{
    public interface IJobApplicationReviewService
    {
        Task<JobApplicationReviewDTO?> GetJobApplicationReviewDTOAsync(string applicationUid, string refNo);
    }
}
