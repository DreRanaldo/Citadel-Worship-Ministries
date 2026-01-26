using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IAnnouncementService
{
    Task<List<Announcement>> GetAllAsync();
    Task<List<Announcement>> GetActiveAsync(bool includeMembersOnly = false);
    Task<Announcement?> GetByIdAsync(int id);
    Task CreateAsync(Announcement announcement);
    Task UpdateAsync(Announcement announcement);
    Task DeleteAsync(int id);
}
