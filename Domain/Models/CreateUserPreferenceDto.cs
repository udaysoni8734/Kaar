namespace Kaar.Domain.Models
{
    public class CreateUserPreferenceDto
    {
        public int UserId { get; set; }
        public string StockSymbol { get; set; }
        public decimal ThresholdPrice { get; set; }
    }
} 