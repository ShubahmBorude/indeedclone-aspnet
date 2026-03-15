using IndeedClone.Modules.SubModules.Auth.Models;

namespace IndeedClone.Modules.SubModules.Auth.ServiceContracts
{
    public interface IRegisterService
    {
        Task Register(string name, string email, string password, string confirmPassword, bool status);
        Task<RegisterModel?> UserExistsAsync(string refNo);
        Task VerifyEmailAsync(string otp, string refNo);
        Task ResendOtpAsync(string refNo);
    }
}
