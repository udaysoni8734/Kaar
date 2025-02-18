// Request DTOs
// Response DTOs
namespace Api.Models.Responses
{
    public class StockPriceResponse
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceChange { get; set; }
        public decimal? PercentageChange { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
