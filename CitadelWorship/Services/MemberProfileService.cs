using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class MemberProfileService : IMemberProfileService
{
    private readonly ApplicationDbContext _context;
    public MemberProfileService(ApplicationDbContext context) => _context = context;

    public async Task<List<MemberProfile>> GetAllAsync() =>
        await _context.MemberProfiles.Include(m => m.User).OrderBy(m => m.User!.LastName).ToListAsync();

    public async Task<MemberProfile?> GetByIdAsync(int id) =>
        await _context.MemberProfiles.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);

    public async Task<MemberProfile?> GetByUserIdAsync(string userId) =>
        await _context.MemberProfiles.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);

    public async Task CreateAsync(MemberProfile profile)
    {
        profile.CreatedAt = DateTime.UtcNow;
        _context.MemberProfiles.Add(profile);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MemberProfile profile)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        _context.MemberProfiles.Update(profile);
        await _context.SaveChangesAsync();
    }
}
