namespace Kaar.Domain.Models
{
    public class StockPriceUpdateDto
    {
        public string Symbol { get; set; }
        public decimal NewPrice { get; set; }
    }
} 