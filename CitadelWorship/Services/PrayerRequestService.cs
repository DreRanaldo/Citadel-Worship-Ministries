using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class PrayerRequestService : IPrayerRequestService
{
    private readonly ApplicationDbContext _context;

    public PrayerRequestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PrayerRequest>> GetAllAsync()
    {
        return await _context.PrayerRequests
            .Include(p => p.SubmittedByUser)
            .OrderByDescending(p => p.SubmittedAt)
            .ToListAsync();
    }

    public async Task<List<PrayerRequest>> GetByUserAsync(string userId)
    {
        return await _context.PrayerRequests
            .Where(p => p.SubmittedByUserId == userId)
            .OrderByDescending(p => p.SubmittedAt)
            .ToListAsync();
    }

    public async Task<PrayerRequest?> GetByIdAsync(int id)
    {
        return await _context.PrayerRequests
            .Include(p => p.SubmittedByUser)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateAsync(PrayerRequest request)
    {
        request.SubmittedAt = DateTime.UtcNow;
        _context.PrayerRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, PrayerRequestStatus status)
    {
        var request = await _context.PrayerRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = status;
            request.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var request = await _context.PrayerRequests.FindAsync(id);
        if (request != null)
        {
            _context.PrayerRequests.Remove(request);
            await _context.SaveChangesAsync();
        }
    }
}
