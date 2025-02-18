// Request DTOs
// Response DTOs
namespace Api.Models.Responses
{
    public class UserPreferenceResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Symbol { get; set; }
        public decimal ThresholdPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public StockPriceResponse CurrentPrice { get; set; }
    }
}
