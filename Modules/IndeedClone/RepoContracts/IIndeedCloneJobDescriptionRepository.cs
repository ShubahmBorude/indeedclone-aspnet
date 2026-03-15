using IndeedClone.Modules.IndeedClone.DTO;

namespace IndeedClone.Modules.IndeedClone.RepoContracts
{
    public interface IIndeedCloneJobDescriptionRepository
    {
        Task<IndeedCloneJobDescriptionDTO?> GetJobDescriptionAsync(string jobUid);
    }
}
