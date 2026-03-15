using IndeedClone.Modules.IndeedClone.DTO;
using IndeedClone.Modules.IndeedClone.RepoContracts;
using IndeedClone.Modules.IndeedClone.ServiceContracts;

namespace IndeedClone.Modules.IndeedClone.Services
{
    public class IndeedCloneHomeService : IIndeedCloneHomeService
    {
        private readonly IIndeedCloneHomeRepository _repository;

        public IndeedCloneHomeService(IIndeedCloneHomeRepository repository)
        {
            _repository = repository;
        }

       
        public async Task<IndeedClonePaginatedJobCardsDTO> GetLatestJobCardsAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetLatestJobCardsAsync(pageNumber, pageSize);
        }

        public async Task<IndeedClonePaginatedJobCardsDTO> GetFilteredJobsAsync(IndeedCloneJobSearchFilterDTO filters, int pageSize = 10)
        {
            return await _repository.GetFilteredJobsAsync(filters, pageSize);
        }
    }
}
