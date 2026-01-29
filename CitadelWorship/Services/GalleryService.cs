using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class GalleryService : IGalleryService
{
    private readonly ApplicationDbContext _context;

    public GalleryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GalleryImage>> GetAllAsync()
    {
        return await _context.GalleryImages
            .Include(g => g.UploadedByUser)
            .OrderByDescending(g => g.UploadedAt)
            .ToListAsync();
    }

    public async Task<List<GalleryImage>> GetActiveAsync()
    {
        return await _context.GalleryImages
            .Where(g => g.IsActive)
            .OrderBy(g => g.DisplayOrder)
            .ThenByDescending(g => g.UploadedAt)
            .ToListAsync();
    }

    public async Task<List<GalleryImage>> GetByCategoryAsync(string category)
    {
        return await _context.GalleryImages
            .Where(g => g.IsActive && g.Category == category)
            .OrderBy(g => g.DisplayOrder)
            .ThenByDescending(g => g.UploadedAt)
            .ToListAsync();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        return await _context.GalleryImages
            .Where(g => g.IsActive && g.Category != null)
            .Select(g => g.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public async Task<GalleryImage?> GetByIdAsync(int id)
    {
        return await _context.GalleryImages
            .Include(g => g.UploadedByUser)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<GalleryImage> CreateAsync(GalleryImage image)
    {
        image.UploadedAt = DateTime.UtcNow;
        _context.GalleryImages.Add(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<GalleryImage> UpdateAsync(GalleryImage image)
    {
        _context.GalleryImages.Update(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var image = await _context.GalleryImages.FindAsync(id);
        if (image == null) return false;

        _context.GalleryImages.Remove(image);
        await _context.SaveChangesAsync();
        return true;
    }
}
