using IndeedClone.Modules.SubModules.JobApplication.Models;

namespace IndeedClone.Modules.SubModules.JobApplication.RepoContracts
{
    public interface IScreenerQuestionsRepository
    {
        Task<ScreenerQuestionsModel?> GetByApplicationUidAsync(string applicationUid);

        Task AddAsync(ScreenerQuestionsModel entity);

        Task UpdateAsync(ScreenerQuestionsModel entity);
    }
}
