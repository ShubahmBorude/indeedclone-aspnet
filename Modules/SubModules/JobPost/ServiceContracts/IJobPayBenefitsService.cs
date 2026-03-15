using IndeedClone.Modules.SubModules.JobPost.DTO;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobPayBenefitsService
    {
        public Task<bool> SavePayBenefitsAsync(JobPayBenefitsDTO dto, string jobUid);
        public Task<JobPayBenefitsDTO?> GetPayBenefitsDTOAsync(string jobUid);
        public Task<int?> GetCurrentPageAsync(string jobUid);
    }
}
