using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class LeaderService : ILeaderService
{
    private readonly ApplicationDbContext _context;

    public LeaderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Leader>> GetAllAsync()
    {
        return await _context.Leaders
            .OrderBy(l => l.DisplayOrder)
            .ThenBy(l => l.LastName)
            .ToListAsync();
    }

    public async Task<List<Leader>> GetActiveAsync()
    {
        return await _context.Leaders
            .Where(l => l.IsActive)
            .OrderBy(l => l.DisplayOrder)
            .ThenBy(l => l.LastName)
            .ToListAsync();
    }

    public async Task<Leader?> GetByIdAsync(int id)
    {
        return await _context.Leaders.FindAsync(id);
    }

    public async Task<Leader> CreateAsync(Leader leader)
    {
        leader.CreatedAt = DateTime.UtcNow;
        _context.Leaders.Add(leader);
        await _context.SaveChangesAsync();
        return leader;
    }

    public async Task<Leader> UpdateAsync(Leader leader)
    {
        leader.UpdatedAt = DateTime.UtcNow;
        _context.Leaders.Update(leader);
        await _context.SaveChangesAsync();
        return leader;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var leader = await _context.Leaders.FindAsync(id);
        if (leader == null) return false;

        _context.Leaders.Remove(leader);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateDisplayOrderAsync(int id, int newOrder)
    {
        var leader = await _context.Leaders.FindAsync(id);
        if (leader == null) return false;

        leader.DisplayOrder = newOrder;
        leader.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
