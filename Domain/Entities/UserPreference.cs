namespace Kaar.Domain.Entities
{
    public class UserPreference
    {
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public string StockSymbol { get; private set; }
        public decimal ThresholdPrice { get; private set; }

        public UserPreference(string userId, string stockSymbol, decimal thresholdPrice)
        {
            UserId = userId;
            StockSymbol = stockSymbol;
            ThresholdPrice = thresholdPrice;
        }

        public void UpdateThreshold(decimal newThresholdPrice)
        {
            ThresholdPrice = newThresholdPrice;
        }
    }
} 