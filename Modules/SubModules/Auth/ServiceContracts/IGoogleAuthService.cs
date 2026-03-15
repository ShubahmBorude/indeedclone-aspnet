using IndeedClone.Modules.SubModules.Auth.DTO;

namespace IndeedClone.Modules.SubModules.Auth.ServiceContracts
{
    public interface IGoogleAuthService
    {
        Task<AuthResponse> HandleSignIn(string token);
    }
}
