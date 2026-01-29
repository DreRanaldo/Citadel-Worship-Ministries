using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface ILeaderService
{
    Task<List<Leader>> GetAllAsync();
    Task<List<Leader>> GetActiveAsync();
    Task<Leader?> GetByIdAsync(int id);
    Task<Leader> CreateAsync(Leader leader);
    Task<Leader> UpdateAsync(Leader leader);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateDisplayOrderAsync(int id, int newOrder);
}
