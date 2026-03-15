using IndeedClone.Emails.Contracts;

namespace IndeedClone.Emails.Queue
{
    public class EmailQueueWorker : BackgroundService
    {
        private readonly IEmailQueue _queue;
        private readonly IEmailSender _sender;

        public EmailQueueWorker(IEmailQueue queue, IEmailSender sender)
        {
            _queue = queue;
            _sender = sender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _queue.DequeueAsync(stoppingToken);
                await _sender.SendAsync(message.To, message.Subject, message.Body);
            }
        }
    }
}
