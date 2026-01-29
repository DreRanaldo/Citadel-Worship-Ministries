using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class SpecialNoteService : ISpecialNoteService
{
    private readonly ApplicationDbContext _context;
    public SpecialNoteService(ApplicationDbContext context) => _context = context;

    public async Task<List<SpecialNote>> GetAllAsync(bool includeConfidential = false)
    {
        var query = _context.SpecialNotes.Include(n => n.RelatedMember).AsQueryable();
        if (!includeConfidential) query = query.Where(n => !n.IsConfidential);
        return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
    }

    public async Task<List<SpecialNote>> GetByMemberAsync(string memberId) =>
        await _context.SpecialNotes.Where(n => n.RelatedMemberId == memberId).OrderByDescending(n => n.CreatedAt).ToListAsync();

    public async Task<List<SpecialNote>> GetByCategoryAsync(NoteCategory category) =>
        await _context.SpecialNotes.Include(n => n.RelatedMember).Where(n => n.Category == category).OrderByDescending(n => n.CreatedAt).ToListAsync();

    public async Task<SpecialNote?> GetByIdAsync(int id) =>
        await _context.SpecialNotes.Include(n => n.RelatedMember).FirstOrDefaultAsync(n => n.Id == id);

    public async Task CreateAsync(SpecialNote note)
    {
        note.CreatedAt = DateTime.UtcNow;
        _context.SpecialNotes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SpecialNote note)
    {
        note.UpdatedAt = DateTime.UtcNow;
        _context.SpecialNotes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var note = await _context.SpecialNotes.FindAsync(id);
        if (note != null) { _context.SpecialNotes.Remove(note); await _context.SaveChangesAsync(); }
    }
}
