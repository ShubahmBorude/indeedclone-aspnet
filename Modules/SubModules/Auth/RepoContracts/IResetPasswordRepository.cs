

using IndeedClone.Modules.SubModules.Auth.Models;

namespace IndeedClone.Modules.SubModules.Auth.RepoContracts
{
    public interface IResetPasswordRepository
    {
        Task CreateAsync(ResetPasswordModel model);
        Task<ResetPasswordModel?> GetActiveOtpAsync(string refNo);
        Task InvalidateActiveTokensAsync(string refNo);
        Task UpdateAsync(ResetPasswordModel model);
    }
}
