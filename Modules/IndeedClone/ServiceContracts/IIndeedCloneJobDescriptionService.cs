using IndeedClone.Modules.IndeedClone.DTO;

namespace IndeedClone.Modules.IndeedClone.ServiceContracts
{
    public interface IIndeedCloneJobDescriptionService
    {
        Task<IndeedCloneJobDescriptionDTO?> GetJobDescriptionAsync(string jobUid);
    }
}
