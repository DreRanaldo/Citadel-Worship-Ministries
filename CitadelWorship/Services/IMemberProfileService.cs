using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IMemberProfileService
{
    Task<List<MemberProfile>> GetAllAsync();
    Task<MemberProfile?> GetByIdAsync(int id);
    Task<MemberProfile?> GetByUserIdAsync(string userId);
    Task CreateAsync(MemberProfile profile);
    Task UpdateAsync(MemberProfile profile);
}
