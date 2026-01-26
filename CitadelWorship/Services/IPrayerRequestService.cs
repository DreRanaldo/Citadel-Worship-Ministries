using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IPrayerRequestService
{
    Task<List<PrayerRequest>> GetAllAsync();
    Task<List<PrayerRequest>> GetByUserAsync(string userId);
    Task<PrayerRequest?> GetByIdAsync(int id);
    Task CreateAsync(PrayerRequest request);
    Task UpdateStatusAsync(int id, PrayerRequestStatus status);
    Task DeleteAsync(int id);
}
