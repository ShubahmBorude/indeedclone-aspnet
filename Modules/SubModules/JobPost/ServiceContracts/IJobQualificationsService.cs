using IndeedClone.Modules.SubModules.JobPost.DTO;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobQualificationsService
    {
        public Task<bool> SaveJobQualificationsAsync(JobQualificationsDTO dto, string jobUid);
        public Task<JobQualificationsDTO?> GetJobQualificationsDTOAsync(string jobUid);
        public Task<int?> GetCurrentPageAsync(string jobUid);
    }
}
