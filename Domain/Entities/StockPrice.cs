namespace Kaar.Domain.Entities
{
    public class StockPrice
    {
        public int Id { get; private set; }
        public string StockSymbol { get; private set; }
        public decimal Price { get; private set; }
        public DateTime Timestamp { get; private set; }

        public StockPrice(string stockSymbol, decimal price)
        {
            StockSymbol = stockSymbol;
            Price = price;
            Timestamp = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal newPrice)
        {
            Price = newPrice;
            Timestamp = DateTime.UtcNow;
        }
    }
} 