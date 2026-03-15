using IndeedClone.Emails.Contracts;
using IndeedClone.Modules.Shared.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace IndeedClone.Emails.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtp;

        public SmtpEmailSender(IOptions<SmtpSettings> options)
        {
            _smtp = options.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                Credentials = new NetworkCredential(
                    _smtp.Email,
                    _smtp.Password),
                EnableSsl = _smtp.EnableSsl
            };

            var message = new MailMessage
            {
                From = new MailAddress(_smtp.Email, "IndeedClone"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            await client.SendMailAsync(message);
        }
    }
}
