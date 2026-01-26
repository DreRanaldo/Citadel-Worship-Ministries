using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IMinistryService
{
    Task<List<Ministry>> GetAllAsync();
    Task<List<Ministry>> GetActiveAsync();
    Task<Ministry?> GetByIdAsync(int id);
    Task CreateAsync(Ministry ministry);
    Task UpdateAsync(Ministry ministry);
    Task DeleteAsync(int id);
}
