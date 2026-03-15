using IndeedClone.Emails.Contracts;
using System.Threading.Channels;

namespace IndeedClone.Emails.Queue
{
    public class EmailQueue : IEmailQueue
    {
        private readonly Channel<EmailMessage> _queue;

        public EmailQueue()
        {
            _queue = Channel.CreateUnbounded<EmailMessage>();
        }

        public async Task EnqueueAsync(EmailMessage message)
        {
            await _queue.Writer.WriteAsync(message);
        }

        public async Task<EmailMessage> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);  
        }

    }
}
