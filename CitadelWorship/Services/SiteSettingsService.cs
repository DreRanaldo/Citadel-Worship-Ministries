using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class SiteSettingsService : ISiteSettingsService
{
    private readonly ApplicationDbContext _context;

    public SiteSettingsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SiteSettings> GetSettingsAsync()
    {
        var settings = await _context.SiteSettings.FirstOrDefaultAsync();
        return settings ?? new SiteSettings();
    }

    public async Task UpdateAsync(SiteSettings settings)
    {
        settings.UpdatedAt = DateTime.UtcNow;
        var existing = await _context.SiteSettings.FirstOrDefaultAsync();
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(settings);
            existing.Id = existing.Id; // Preserve the ID
        }
        else
        {
            _context.SiteSettings.Add(settings);
        }
        await _context.SaveChangesAsync();
    }
}
