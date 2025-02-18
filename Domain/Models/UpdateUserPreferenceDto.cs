namespace Kaar.Domain.Models
{
    public class UpdateUserPreferenceDto
    {
        public int UserId { get; set; }
        public string StockSymbol { get; set; }
        public decimal NewThresholdPrice { get; set; }
    }
} 