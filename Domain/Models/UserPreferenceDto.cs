namespace Kaar.Domain.Models
{
    public class UserPreferenceDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StockSymbol { get; set; }
        public decimal ThresholdPrice { get; set; }
    }
} 