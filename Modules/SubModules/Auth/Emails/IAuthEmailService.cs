namespace IndeedClone.Modules.SubModules.Auth.Emails
{
    public interface IAuthEmailService
    {
        Task SendVerificationEmailAsync(string email, string name, string otp);
    }
}
