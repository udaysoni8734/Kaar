using Kaar.Domain.Models;

namespace Application.Services
{
    public interface IUserPreferenceService
    {
        Task<IEnumerable<UserPreferenceDto>> GetUserPreferencesAsync(int userId);
        Task<UserPreferenceDto> GetPreferenceByIdAsync(int id);
        Task<IEnumerable<UserPreferenceDto>> GetBySymbolAsync(string symbol);
        Task<UserPreferenceDto> CreatePreferenceAsync(CreateUserPreferenceDto preference);
        Task UpdatePreferenceAsync(int id, UpdateUserPreferenceDto preference);
        Task DeletePreferenceAsync(int id);
        Task<IEnumerable<UserPreferenceDto>> GetAllPreferencesAsync();
    }
} 