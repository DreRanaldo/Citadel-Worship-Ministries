using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class EducationRequestService : IEducationRequestService
{
    private readonly ApplicationDbContext _context;
    public EducationRequestService(ApplicationDbContext context) => _context = context;

    public async Task<List<EducationRequest>> GetAllAsync() =>
        await _context.EducationRequests.Include(e => e.SubmittedByUser).OrderByDescending(e => e.SubmittedAt).ToListAsync();

    public async Task<List<EducationRequest>> GetByUserAsync(string userId) =>
        await _context.EducationRequests.Where(e => e.SubmittedByUserId == userId).OrderByDescending(e => e.SubmittedAt).ToListAsync();

    public async Task<List<EducationRequest>> GetByStatusAsync(RequestStatus status) =>
        await _context.EducationRequests.Include(e => e.SubmittedByUser).Where(e => e.Status == status).OrderByDescending(e => e.SubmittedAt).ToListAsync();

    public async Task<EducationRequest?> GetByIdAsync(int id) =>
        await _context.EducationRequests.Include(e => e.SubmittedByUser).FirstOrDefaultAsync(e => e.Id == id);

    public async Task CreateAsync(EducationRequest request)
    {
        request.SubmittedAt = DateTime.UtcNow;
        _context.EducationRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EducationRequest request)
    {
        request.UpdatedAt = DateTime.UtcNow;
        _context.EducationRequests.Update(request);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, RequestStatus status, string? notes = null)
    {
        var request = await _context.EducationRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = status;
            request.UpdatedAt = DateTime.UtcNow;
            if (notes != null) request.InternalNotes = notes;
            if (status == RequestStatus.Completed) request.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
