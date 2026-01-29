using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface ISpecialNoteService
{
    Task<List<SpecialNote>> GetAllAsync(bool includeConfidential = false);
    Task<List<SpecialNote>> GetByMemberAsync(string memberId);
    Task<List<SpecialNote>> GetByCategoryAsync(NoteCategory category);
    Task<SpecialNote?> GetByIdAsync(int id);
    Task CreateAsync(SpecialNote note);
    Task UpdateAsync(SpecialNote note);
    Task DeleteAsync(int id);
}
