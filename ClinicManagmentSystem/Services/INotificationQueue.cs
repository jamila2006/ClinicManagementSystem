namespace ClinicManagementSystem.Services
{
    public record EmailNotification(string ToEmail, string Subject, string Body);

    public interface INotificationQueue
    {
        void Enqueue(EmailNotification notification);
        Task<EmailNotification> DequeueAsync(CancellationToken cancellationToken);
    }
}