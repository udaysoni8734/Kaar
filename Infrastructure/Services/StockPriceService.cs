using Kaar.Domain.Models;
using AutoMapper;
using Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Kaar.Infrastructure.Database.Context;
using Kaar.Domain.Entities;
using Kaar.Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class StockPriceService : IStockPriceService
    {
        private readonly StockTrackingContext _context;
        private readonly ILogger<StockPriceService> _logger;
        private readonly IMapper _mapper;

        public StockPriceService(
            StockTrackingContext context,
            ILogger<StockPriceService> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StockPriceDto>> GetLatestPricesAsync(int limit)
        {
            try
            {
                var stockPrices = await _context.StockPrices
                    .OrderByDescending(x => x.Timestamp)
                    .Take(limit)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<StockPriceDto>>(stockPrices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest stock prices");
                throw;
            }
        }

        public async Task<StockPriceDto> GetLatestPriceAsync(string symbol)
        {
            try
            {
                var stockPrice = await _context.StockPrices
                    .FirstOrDefaultAsync(x => x.StockSymbol == symbol);

                return _mapper.Map<StockPriceDto>(stockPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stock price for symbol: {Symbol}", symbol);
                throw;
            }
        }

        public async Task<StockPriceDto> AddPriceAsync(StockPriceDto stockPriceDto)
        {
            try
            {
                var stockPrice = new StockPrice(stockPriceDto.Symbol, stockPriceDto.Price);
                await _context.StockPrices.AddAsync(stockPrice);
                await _context.SaveChangesAsync();

                return _mapper.Map<StockPriceDto>(stockPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock price for symbol: {Symbol}", stockPriceDto.Symbol);
                throw;
            }
        }

        public async Task UpdatePriceAsync(StockPriceUpdateDto updateDto)
        {
            try
            {
                var stockPrice = await _context.StockPrices
                    .FirstOrDefaultAsync(x => x.StockSymbol == updateDto.Symbol);

                if (stockPrice == null)
                {
                    throw new NotFoundException($"Stock price not found for symbol: {updateDto.Symbol}");
                }

                stockPrice.UpdatePrice(updateDto.NewPrice);
                await _context.SaveChangesAsync();

                await CheckThresholdsAndNotifyAsync(updateDto.Symbol, updateDto.NewPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock price for symbol: {Symbol}", updateDto.Symbol);
                throw;
            }
        }

        public async Task DeletePriceAsync(string symbol)
        {
            try
            {
                var stockPrice = await _context.StockPrices
                    .FirstOrDefaultAsync(x => x.StockSymbol == symbol);

                if (stockPrice == null)
                {
                    throw new NotFoundException($"Stock price not found for symbol: {symbol}");
                }

                _context.StockPrices.Remove(stockPrice);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting stock price for symbol: {Symbol}", symbol);
                throw;
            }
        }

        public async Task CheckThresholdsAndNotifyAsync(string symbol, decimal currentPrice)
        {
            try
            {
                var preferences = await _context.UserPreferences
                    .Where(p => p.StockSymbol == symbol && p.ThresholdPrice <= currentPrice)
                    .ToListAsync();

                foreach (var preference in preferences)
                {
                    // In a real application, you would implement actual notification logic here
                    _logger.LogInformation(
                        "Price Alert: Stock {Symbol} has reached {CurrentPrice}, crossing threshold {ThresholdPrice} for user {UserId}",
                        symbol,
                        currentPrice,
                        preference.ThresholdPrice,
                        preference.UserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking thresholds for symbol: {Symbol}", symbol);
                throw;
            }
        }
    }
} 