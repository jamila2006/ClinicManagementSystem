using System.Threading.Channels;

namespace ClinicManagementSystem.Services
{
    public class NotificationQueue : INotificationQueue
    {
        private readonly Channel<EmailNotification> _channel =
            Channel.CreateUnbounded<EmailNotification>();

        public void Enqueue(EmailNotification notification)
        {
            _channel.Writer.TryWrite(notification);
        }

        public async Task<EmailNotification> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
