using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface ISermonService
{
    Task<List<Sermon>> GetAllAsync();
    Task<List<Sermon>> GetPublishedAsync();
    Task<Sermon?> GetByIdAsync(int id);
    Task<List<Sermon>> SearchAsync(string? query, string? series);
    Task<List<string>> GetSeriesListAsync();
    Task CreateAsync(Sermon sermon);
    Task UpdateAsync(Sermon sermon);
    Task DeleteAsync(int id);
}
