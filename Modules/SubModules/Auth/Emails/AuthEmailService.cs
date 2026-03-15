using IndeedClone.Emails.Contracts;
using IndeedClone.Emails.EmailTemplates;
using IndeedClone.Emails.Queue;


namespace IndeedClone.Modules.SubModules.Auth.Emails
{
    public class AuthEmailService : IAuthEmailService
    {
        private readonly IEmailQueue _queue;

        public AuthEmailService(IEmailQueue queue)
        {
            _queue = queue;
        }

     // # Business email (OTP)
        public async Task SendVerificationEmailAsync(string email, string name, string otp)
        {
            var subject = "Verify Your Email - IndeedClone";
            var body = AuthEmailTemplates.BuidVerificationOTP(name, otp);

            await _queue.EnqueueAsync(new EmailMessage(email, subject, body));
        }
    }
}
