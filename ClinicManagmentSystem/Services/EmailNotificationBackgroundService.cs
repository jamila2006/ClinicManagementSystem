namespace ClinicManagementSystem.Services
{
    public class EmailNotificationBackgroundService : BackgroundService
    {
        private readonly INotificationQueue _queue;
        private readonly ILogger<EmailNotificationBackgroundService> _logger;

        public EmailNotificationBackgroundService(INotificationQueue queue, ILogger<EmailNotificationBackgroundService> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var notification = await _queue.DequeueAsync(stoppingToken);
                    await SendEmailAsync(notification);
                }
                catch (OperationCanceledException)
                {
                    break; // tətbiq bağlanır, normal haldır
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Email göndərilməsi zamanı xəta baş verdi");
                }
            }
        }

        private async Task SendEmailAsync(EmailNotification notification)
        {
            // Real SMTP yoxdur — simulyasiya edirik (gecikməni təqlid edirik)
            await Task.Delay(1000);
            _logger.LogInformation("[EMAIL SIMULYASIYA] Kimə: {To}, Mövzu: {Subject}", notification.ToEmail, notification.Subject);
        }
    }
}
