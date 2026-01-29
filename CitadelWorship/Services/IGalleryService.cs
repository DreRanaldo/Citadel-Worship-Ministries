using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IGalleryService
{
    Task<List<GalleryImage>> GetAllAsync();
    Task<List<GalleryImage>> GetActiveAsync();
    Task<List<GalleryImage>> GetByCategoryAsync(string category);
    Task<List<string>> GetCategoriesAsync();
    Task<GalleryImage?> GetByIdAsync(int id);
    Task<GalleryImage> CreateAsync(GalleryImage image);
    Task<GalleryImage> UpdateAsync(GalleryImage image);
    Task<bool> DeleteAsync(int id);
}
