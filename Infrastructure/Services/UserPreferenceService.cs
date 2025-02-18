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
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly StockTrackingContext _context;
        private readonly ILogger<UserPreferenceService> _logger;
        private readonly IMapper _mapper;

        public UserPreferenceService(
            StockTrackingContext context,
            ILogger<UserPreferenceService> logger,
            IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserPreferenceDto>> GetUserPreferencesAsync(int userId)
        {
            try
            {
                var preferences = await _context.UserPreferences
                    .Where(p => p.UserId == userId.ToString())
                    .ToListAsync();

                return _mapper.Map<IEnumerable<UserPreferenceDto>>(preferences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving preferences for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<UserPreferenceDto> GetPreferenceByIdAsync(int id)
        {
            try
            {
                var preference = await _context.UserPreferences
                    .FirstOrDefaultAsync(p => p.Id == id);

                return _mapper.Map<UserPreferenceDto>(preference);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving preference with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<UserPreferenceDto>> GetBySymbolAsync(string symbol)
        {
            try
            {
                var preferences = await _context.UserPreferences
                    .Where(p => p.StockSymbol == symbol)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<UserPreferenceDto>>(preferences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving preferences for symbol: {Symbol}", symbol);
                throw;
            }
        }

        public async Task<UserPreferenceDto> CreatePreferenceAsync(CreateUserPreferenceDto preference)
        {
            try
            {
                var userPreference = new UserPreference(
                    preference.UserId.ToString(),
                    preference.StockSymbol,
                    preference.ThresholdPrice
                );

                await _context.UserPreferences.AddAsync(userPreference);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created preference for user {UserId} for stock {Symbol}", 
                    preference.UserId, preference.StockSymbol);

                return _mapper.Map<UserPreferenceDto>(userPreference);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating preference for user {UserId}", preference.UserId);
                throw;
            }
        }

        public async Task UpdatePreferenceAsync(int id, UpdateUserPreferenceDto preference)
        {
            try
            {
                var existingPreference = await _context.UserPreferences
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (existingPreference == null)
                {
                    _logger.LogWarning("Preference not found with ID: {Id}", id);
                    throw new NotFoundException($"Preference not found with ID: {id}");
                }

                existingPreference.UpdateThreshold(preference.NewThresholdPrice);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Updated preference {Id} with new threshold {Threshold}", 
                    id, preference.NewThresholdPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating preference {Id}", id);
                throw;
            }
        }

        public async Task DeletePreferenceAsync(int id)
        {
            try
            {
                var preference = await _context.UserPreferences
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (preference == null)
                {
                    _logger.LogWarning("Preference not found with ID: {Id}", id);
                    throw new NotFoundException($"Preference not found with ID: {id}");
                }

                _context.UserPreferences.Remove(preference);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted preference {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting preference {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<UserPreferenceDto>> GetAllPreferencesAsync()
        {
            var preferences = await _context.UserPreferences
                .AsNoTracking()  // For better performance since we're only reading
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserPreferenceDto>>(preferences);
        }
    }
} 