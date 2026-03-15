

namespace IndeedClone.Modules.SubModules.Auth.ServiceContracts
{
    public interface IResetPasswordService
    {
        Task RequestResetAsync(string email);
        Task VerifyOtpAsync(string refNo, string otp);
        Task ResetPasswordAsync(string refNo, string newPassword, string confirmPassword);
    }
}
