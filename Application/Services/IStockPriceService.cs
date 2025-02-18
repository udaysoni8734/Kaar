using Kaar.Domain.Models;

namespace Application.Services;

public interface IStockPriceService
{
    Task<IEnumerable<StockPriceDto>> GetLatestPricesAsync(int limit);
    Task<StockPriceDto> GetLatestPriceAsync(string symbol);
    Task UpdatePriceAsync(StockPriceUpdateDto updateDto);
    Task<StockPriceDto> AddPriceAsync(StockPriceDto stockPrice);
    Task DeletePriceAsync(string symbol);
    Task CheckThresholdsAndNotifyAsync(string symbol, decimal currentPrice);
}
