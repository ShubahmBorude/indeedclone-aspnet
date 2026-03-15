using IndeedClone.Modules.SubModules.JobApplication.Models;

namespace IndeedClone.Modules.SubModules.JobApplication.RepoContracts
{
    public interface IRelExperienceRepository
    {
        Task<RelExperienceModel?> GetByApplicationUidAsync(string applicationUid);

        Task AddAsync(RelExperienceModel entity);

        Task UpdateAsync(RelExperienceModel entity);
    }
}
