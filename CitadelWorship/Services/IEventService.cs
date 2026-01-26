using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IEventService
{
    Task<List<Event>> GetAllAsync();
    Task<List<Event>> GetUpcomingAsync(bool includeMembersOnly = false);
    Task<Event?> GetByIdAsync(int id);
    Task CreateAsync(Event evt);
    Task UpdateAsync(Event evt);
    Task DeleteAsync(int id);
}
