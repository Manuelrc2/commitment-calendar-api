using commitment_calendar_api.Interfaces;
using commitment_calendar_api.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Runtime;
using System.Threading;

namespace commitment_calendar_api.Services
{
    public class NotificationService : HostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer? _timer;
        
        public NotificationService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var now = DateTime.Now;
                var nextMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0).AddMinutes(1);
                var initialDelay = nextMinute - now;

                //_logger.LogInformation("First notification check scheduled for {NextCheck} (in {DelaySeconds:F1} seconds)", nextMinute.ToString("HH:mm:ss"), initialDelay.TotalSeconds);

                _timer = new Timer(
                    callback: async state => await ProcessEmailsInScope(cancellationToken),
                    state: null,
                    dueTime: initialDelay,
                    period: TimeSpan.FromMinutes(1)
                );

                cancellationToken.Register(() =>
                {
                    //_logger.LogInformation("Notification service shutdown requested");
                    _timer?.Change(Timeout.Infinite, 0); // Stop the timer
                });

                //_logger.LogInformation("Notification service initialized successfully");
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                //_logger.LogError(ex, "Failed to initialize notification service");
                throw new Exception("Failed to initialize notification service");
            }
        }

        private async Task ProcessEmailsInScope(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await emailService.ProcessEmails(cancellationToken);
            }
        }
    }
}
