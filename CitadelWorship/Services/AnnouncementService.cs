using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly ApplicationDbContext _context;

    public AnnouncementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Announcement>> GetAllAsync()
    {
        return await _context.Announcements
            .OrderByDescending(a => a.PublishDate)
            .ToListAsync();
    }

    public async Task<List<Announcement>> GetActiveAsync(bool includeMembersOnly = false)
    {
        var now = DateTime.UtcNow;
        var query = _context.Announcements
            .Where(a => a.IsPublished && a.PublishDate <= now &&
                        (a.ExpiryDate == null || a.ExpiryDate > now));

        if (!includeMembersOnly)
        {
            query = query.Where(a => !a.IsMembersOnly);
        }

        return await query.OrderByDescending(a => a.PublishDate).ToListAsync();
    }

    public async Task<Announcement?> GetByIdAsync(int id)
    {
        return await _context.Announcements.FindAsync(id);
    }

    public async Task CreateAsync(Announcement announcement)
    {
        announcement.CreatedAt = DateTime.UtcNow;
        _context.Announcements.Add(announcement);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Announcement announcement)
    {
        announcement.UpdatedAt = DateTime.UtcNow;
        _context.Announcements.Update(announcement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var announcement = await _context.Announcements.FindAsync(id);
        if (announcement != null)
        {
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
        }
    }
}
