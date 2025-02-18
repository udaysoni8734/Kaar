namespace Kaar.Domain.Entities
{
    public class UserPreference
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string StockSymbol { get; set; }
        public decimal ThresholdPrice { get; set; }

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