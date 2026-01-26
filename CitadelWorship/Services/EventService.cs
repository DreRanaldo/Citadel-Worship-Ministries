using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _context.Events
            .OrderByDescending(e => e.StartDate)
            .ToListAsync();
    }

    public async Task<List<Event>> GetUpcomingAsync(bool includeMembersOnly = false)
    {
        var query = _context.Events
            .Where(e => e.IsPublished && e.StartDate >= DateTime.UtcNow.Date);

        if (!includeMembersOnly)
        {
            query = query.Where(e => !e.IsMembersOnly);
        }

        return await query.OrderBy(e => e.StartDate).ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task CreateAsync(Event evt)
    {
        evt.CreatedAt = DateTime.UtcNow;
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event evt)
    {
        evt.UpdatedAt = DateTime.UtcNow;
        _context.Events.Update(evt);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt != null)
        {
            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
        }
    }
}
