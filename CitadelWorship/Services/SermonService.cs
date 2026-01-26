using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class SermonService : ISermonService
{
    private readonly ApplicationDbContext _context;

    public SermonService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sermon>> GetAllAsync()
    {
        return await _context.Sermons
            .OrderByDescending(s => s.DatePreached)
            .ToListAsync();
    }

    public async Task<List<Sermon>> GetPublishedAsync()
    {
        return await _context.Sermons
            .Where(s => s.IsPublished)
            .OrderByDescending(s => s.DatePreached)
            .ToListAsync();
    }

    public async Task<Sermon?> GetByIdAsync(int id)
    {
        return await _context.Sermons.FindAsync(id);
    }

    public async Task<List<Sermon>> SearchAsync(string? query, string? series)
    {
        var q = _context.Sermons.Where(s => s.IsPublished).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var lower = query.ToLower();
            q = q.Where(s =>
                s.Title.ToLower().Contains(lower) ||
                s.Speaker.ToLower().Contains(lower) ||
                (s.ScriptureReference != null && s.ScriptureReference.ToLower().Contains(lower)) ||
                (s.Description != null && s.Description.ToLower().Contains(lower)));
        }

        if (!string.IsNullOrWhiteSpace(series))
        {
            q = q.Where(s => s.Series == series);
        }

        return await q.OrderByDescending(s => s.DatePreached).ToListAsync();
    }

    public async Task<List<string>> GetSeriesListAsync()
    {
        return await _context.Sermons
            .Where(s => s.IsPublished && s.Series != null)
            .Select(s => s.Series!)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync();
    }

    public async Task CreateAsync(Sermon sermon)
    {
        sermon.CreatedAt = DateTime.UtcNow;
        _context.Sermons.Add(sermon);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sermon sermon)
    {
        sermon.UpdatedAt = DateTime.UtcNow;
        _context.Sermons.Update(sermon);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sermon = await _context.Sermons.FindAsync(id);
        if (sermon != null)
        {
            _context.Sermons.Remove(sermon);
            await _context.SaveChangesAsync();
        }
    }
}
