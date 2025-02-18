namespace Kaar.Domain.Models
{
    public class StockPriceDto
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 