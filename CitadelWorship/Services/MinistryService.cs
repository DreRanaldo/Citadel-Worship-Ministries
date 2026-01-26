using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class MinistryService : IMinistryService
{
    private readonly ApplicationDbContext _context;

    public MinistryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ministry>> GetAllAsync()
    {
        return await _context.Ministries
            .OrderBy(m => m.SortOrder)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<List<Ministry>> GetActiveAsync()
    {
        return await _context.Ministries
            .Where(m => m.IsActive)
            .OrderBy(m => m.SortOrder)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<Ministry?> GetByIdAsync(int id)
    {
        return await _context.Ministries.FindAsync(id);
    }

    public async Task CreateAsync(Ministry ministry)
    {
        ministry.CreatedAt = DateTime.UtcNow;
        _context.Ministries.Add(ministry);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ministry ministry)
    {
        ministry.UpdatedAt = DateTime.UtcNow;
        _context.Ministries.Update(ministry);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var ministry = await _context.Ministries.FindAsync(id);
        if (ministry != null)
        {
            _context.Ministries.Remove(ministry);
            await _context.SaveChangesAsync();
        }
    }
}
