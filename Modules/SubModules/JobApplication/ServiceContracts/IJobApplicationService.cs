using IndeedClone.Modules.SubModules.JobApplication.DTO;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Models;


namespace IndeedClone.Modules.SubModules.JobApplication.ServiceContracts
{
    public interface IJobApplicationService
    {
     // # CV FILE UPLOAD
        Task<CVDTO?> GetCVDTOAsync(string refNo, string applicationUid);
        Task<bool> UploadCVAsync(string refNo, string applicationUid, IFormFile file);
        Task<(byte[] fileBytes, string fileName)?> DownloadCVAsync(string refNo, string applicationUid);

     // # APPLY Business Logic
        Task<string?> HandleApplyAsync(string refNo, string jobUid, string? applicationUid = null);

        Task<JobApplicationModel?> ValidateAccessAsync(string refNo, string applicationUid);

        Task UpdateProgressAsync(string ApplicationUid, JobApplicationPages nextPage);

        Task<JobApplicationModel?> GetByApplicationUid(string ApplicationUid);

    }
}
