using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface ISiteSettingsService
{
    Task<SiteSettings> GetSettingsAsync();
    Task UpdateAsync(SiteSettings settings);
}
