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

        public async Task<UserPreferenceDto> CreatePreferenceAsync(CreateUserPreferenceDto preferenceDto)
        {
            try
            {
                var preference = new UserPreference(
                    preferenceDto.UserId.ToString(),
                    preferenceDto.StockSymbol,
                    preferenceDto.ThresholdPrice);

                await _context.UserPreferences.AddAsync(preference);
                await _context.SaveChangesAsync();

                return _mapper.Map<UserPreferenceDto>(preference);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating preference for user: {UserId}", preferenceDto.UserId);
                throw;
            }
        }

        public async Task UpdatePreferenceAsync(int id, UpdateUserPreferenceDto preferenceDto)
        {
            try
            {
                var preference = await _context.UserPreferences
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (preference == null)
                {
                    throw new NotFoundException($"Preference not found with ID: {id}");
                }

                preference.UpdateThreshold(preferenceDto.NewThresholdPrice);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating preference with ID: {Id}", id);
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
                    throw new NotFoundException($"Preference not found with ID: {id}");
                }

                _context.UserPreferences.Remove(preference);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting preference with ID: {Id}", id);
                throw;
            }
        }
    }
} 