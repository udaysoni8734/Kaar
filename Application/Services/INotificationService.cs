namespace Application.Services
{
    public interface INotificationService
    {
        Task SendAlertAsync(int userId, string message);
    }
} 