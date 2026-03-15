using IndeedClone.Modules.SubModules.JobApplication.Enums;

namespace IndeedClone.Modules.SubModules.JobApplication.ServiceContracts
{
    public interface IJobApplicationProgressService
    {
        Task<JobApplicationPages?> GetLastSavedPageAsync(string applicationUid, string refNo);

    }
}
