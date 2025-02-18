using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;

namespace Kaar.BackgroundServices.Services
{
    public class StockPriceMonitorService : BackgroundService
    {
        private readonly ILogger<StockPriceMonitorService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30); // Check every 30 seconds

        public StockPriceMonitorService(
            ILogger<StockPriceMonitorService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        await CheckStockPricesAsync(scope.ServiceProvider);
                    }
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while monitoring stock prices");
                }
            }
        }

        private async Task CheckStockPricesAsync(IServiceProvider serviceProvider)
        {
            var userPreferenceService = serviceProvider.GetRequiredService<IUserPreferenceService>();
            var stockPriceService = serviceProvider.GetRequiredService<IStockPriceService>();
            var notificationService = serviceProvider.GetRequiredService<INotificationService>();

            var preferences = await userPreferenceService.GetAllPreferencesAsync();
            
            foreach (var preference in preferences)
            {
                var currentPrice = await stockPriceService.GetLatestPriceAsync(preference.StockSymbol);
                
                if (currentPrice != null && HasCrossedThreshold(currentPrice.Price, preference.ThresholdPrice))
                {
                    await notificationService.SendAlertAsync(
                        preference.UserId,
                        $"Alert: {preference.StockSymbol} price ({currentPrice.Price:C}) has crossed your threshold ({preference.ThresholdPrice:C})");
                }
            }
        }

        private bool HasCrossedThreshold(decimal currentPrice, decimal thresholdPrice)
        {
            // You can implement more sophisticated threshold logic here
            return currentPrice >= thresholdPrice;
        }
    }
} 