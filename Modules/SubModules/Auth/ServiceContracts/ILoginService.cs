using IndeedClone.Modules.SubModules.Auth.DTO;

namespace IndeedClone.Modules.SubModules.Auth.ServiceContracts
{
    public interface ILoginService
    {
        Task<AuthResponse> Login(string email, string password);
        Task<string> UserName(string email);
    }
}
