using IndeedClone.Modules.IndeedClone.DTO;

namespace IndeedClone.Modules.IndeedClone.RepoContracts
{
    public interface IIndeedCloneHomeRepository
    {
        Task<IndeedClonePaginatedJobCardsDTO> GetLatestJobCardsAsync(int pageNumber = 1, int pageSize = 10);
        Task<IndeedClonePaginatedJobCardsDTO> GetFilteredJobsAsync(IndeedCloneJobSearchFilterDTO filters, int pageSize = 10);
    }
}
