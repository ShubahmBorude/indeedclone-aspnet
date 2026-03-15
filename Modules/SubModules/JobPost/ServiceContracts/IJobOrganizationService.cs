using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Models;

namespace IndeedClone.Modules.SubModules.JobPost.ServiceContracts
{
    public interface IJobOrganizationService
    {
        public Task<bool> SaveOrganizationAsync(JobOrganizationDTO dto, string refNo, string? jobUid);
        public Task<JobOrganizationDTO?> GetOrganizationDTOAsync(string jobUid, string refNo);
        public Task<JobOrganizationModel?> GetByJobUidAsync(string jobUid);
        public Task<string?> GetLatestDraftsJobUidAsync(string refNo);
        public Task<int?> GetCurrentPageAsync(string jobUid);

    }
}
