using IndeedClone.Modules.SubModules.JobApplication.DTO;

namespace IndeedClone.Modules.SubModules.JobApplication.ServiceContracts
{
    public interface IRelExperienceService
    {
        public Task<bool> SaveExperienceAsync(RelExperienceDTO dto, string applicationUid);

        public Task<RelExperienceDTO?> GetRelExperienceDTOAsync(string applicationUid);

    }
}
