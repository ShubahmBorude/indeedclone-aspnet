
using IndeedClone.Modules.SubModules.JobApplication.DTO;
using IndeedClone.Modules.SubModules.JobApplication.Enums;

namespace IndeedClone.Modules.SubModules.JobApplication.ServiceContracts
{
    public interface IScreenerQuestionsService
    {
        Task<ScreenerQuestionsResult> SaveScreenerQuestionsAsync(ScreenerQuestionsDTO dto, string applicationUid);

        Task<ScreenerQuestionsDTO?> GetScreenerQuestionsDTOAsync(string applicationUid);

    }
}
