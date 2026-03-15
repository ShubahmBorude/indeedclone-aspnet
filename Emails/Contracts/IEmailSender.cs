namespace IndeedClone.Emails.Contracts
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body);
    }
}
