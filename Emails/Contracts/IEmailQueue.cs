using IndeedClone.Emails.Queue;

namespace IndeedClone.Emails.Contracts
{
    public interface IEmailQueue
    {
        Task EnqueueAsync(EmailMessage message);

        Task<EmailMessage> DequeueAsync(CancellationToken cancellationToken);
    }
}
