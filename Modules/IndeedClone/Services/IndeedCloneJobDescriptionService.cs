using IndeedClone.Modules.IndeedClone.DTO;
using IndeedClone.Modules.IndeedClone.RepoContracts;
using IndeedClone.Modules.IndeedClone.ServiceContracts;

namespace IndeedClone.Modules.IndeedClone.Services
{
    public class IndeedCloneJobDescriptionService : IIndeedCloneJobDescriptionService
    {
        private readonly IIndeedCloneJobDescriptionRepository _repository;

        public IndeedCloneJobDescriptionService(IIndeedCloneJobDescriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IndeedCloneJobDescriptionDTO?> GetJobDescriptionAsync(string jobUid)
        {
            return await _repository.GetJobDescriptionAsync(jobUid);
        }
    }
}
