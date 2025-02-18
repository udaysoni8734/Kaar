using Application.Services;
using Microsoft.Extensions.Logging;

namespace Kaar.BackgroundServices.Services
{
    public class ConsoleNotificationService : INotificationService
    {
        private readonly ILogger<ConsoleNotificationService> _logger;

        public ConsoleNotificationService(ILogger<ConsoleNotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendAlertAsync(int userId, string message)
        {
            // In a real application, this would send an email or push notification
            _logger.LogInformation("Notification for User {UserId}: {Message}", userId, message);
            Console.WriteLine($"[{DateTime.Now}] Alert for User {userId}: {message}");
            
            return Task.CompletedTask;
        }
    }
} 