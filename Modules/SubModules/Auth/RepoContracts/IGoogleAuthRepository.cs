using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth.GoogleJwtPayload;
using IndeedClone.Modules.SubModules.Auth.Models;

namespace IndeedClone.Modules.SubModules.Auth.RepoContracts
{
    public interface IGoogleAuthRepository
    {
        Task<GoogleAuthModel?> GetByProviderUserIdAsync(string userId);
        Task<GoogleAuthModel?> GetByEmailAsync(string email);
        Task<GoogleAuthModel?> GetByCompositeKeyAsync(string refNo, string issuer, AuthProvider providedId);
        Task<GoogleAuthModel?> CreateFromPayloadAsync(GoogleJwtPayload payload, string refNo, GoogleAuthStatus status);
        Task Update(GoogleAuthModel googleAuth);
        Task SoftDelete(GoogleAuthModel googleAuth);
        Task<List<GoogleAuthModel>> GetAllActiveByRefNo(string refNo);

    }

}
