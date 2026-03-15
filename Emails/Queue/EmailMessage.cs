namespace IndeedClone.Emails.Queue
{
    public record EmailMessage(string To, string Subject, string Body)
    {
    }
}
