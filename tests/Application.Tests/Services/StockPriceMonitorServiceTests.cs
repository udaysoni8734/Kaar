using Xunit;
using Moq;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Kaar.BackgroundServices.Services;
using Microsoft.Extensions.Logging;
using Kaar.Domain.Models;

namespace Application.Tests.Services
{
    public class StockPriceMonitorServiceTests
    {
        private readonly Mock<ILogger<StockPriceMonitorService>> _loggerMock;
        private readonly Mock<IStockPriceService> _stockPriceServiceMock;
        private readonly Mock<IUserPreferenceService> _userPreferenceServiceMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IServiceScopeFactory> _scopeFactoryMock;

        public StockPriceMonitorServiceTests()
        {
            _loggerMock = new Mock<ILogger<StockPriceMonitorService>>();
            _stockPriceServiceMock = new Mock<IStockPriceService>();
            _userPreferenceServiceMock = new Mock<IUserPreferenceService>();
            _notificationServiceMock = new Mock<INotificationService>();
            _scopeFactoryMock = new Mock<IServiceScopeFactory>();

            var scopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IStockPriceService)))
                .Returns(_stockPriceServiceMock.Object);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(IUserPreferenceService)))
                .Returns(_userPreferenceServiceMock.Object);
            serviceProviderMock
                .Setup(x => x.GetService(typeof(INotificationService)))
                .Returns(_notificationServiceMock.Object);

            scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
            _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
        }

        [Fact]
        public async Task CheckStockPrices_WhenPriceExceedsThreshold_SendsNotification()
        {
            // Arrange
            var preferences = new List<UserPreferenceDto>
            {
                new() { UserId = 1, StockSymbol = "AAPL", ThresholdPrice = 150 }
            };

            var currentPrice = new StockPriceDto 
            { 
                Symbol = "AAPL", 
                Price = 160, 
                Timestamp = DateTime.UtcNow 
            };

            _userPreferenceServiceMock
                .Setup(x => x.GetAllPreferencesAsync())
                .ReturnsAsync(preferences);

            _stockPriceServiceMock
                .Setup(x => x.GetLatestPriceAsync("AAPL"))
                .ReturnsAsync(currentPrice);

            var service = new StockPriceMonitorService(_loggerMock.Object, _scopeFactoryMock.Object);

            // Act
            await service.StartAsync(CancellationToken.None);
            await Task.Delay(100); // Allow time for the background service to run
            await service.StopAsync(CancellationToken.None);

            // Assert
            _notificationServiceMock.Verify(
                x => x.SendAlertAsync(
                    It.Is<int>(id => id == 1),
                    It.Is<string>(msg => msg.Contains("AAPL") && msg.Contains("160"))),
                Times.Once);
        }

        [Fact]
        public async Task CheckStockPrices_WhenPriceBelowThreshold_DoesNotSendNotification()
        {
            // Arrange
            var preferences = new List<UserPreferenceDto>
            {
                new() { UserId = 1, StockSymbol = "AAPL", ThresholdPrice = 150 }
            };

            var currentPrice = new StockPriceDto 
            { 
                Symbol = "AAPL", 
                Price = 140, 
                Timestamp = DateTime.UtcNow 
            };

            _userPreferenceServiceMock
                .Setup(x => x.GetAllPreferencesAsync())
                .ReturnsAsync(preferences);

            _stockPriceServiceMock
                .Setup(x => x.GetLatestPriceAsync("AAPL"))
                .ReturnsAsync(currentPrice);

            var service = new StockPriceMonitorService(_loggerMock.Object, _scopeFactoryMock.Object);

            // Act
            await service.StartAsync(CancellationToken.None);
            await Task.Delay(100);
            await service.StopAsync(CancellationToken.None);

            // Assert
            _notificationServiceMock.Verify(
                x => x.SendAlertAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never);
        }
    }
} 