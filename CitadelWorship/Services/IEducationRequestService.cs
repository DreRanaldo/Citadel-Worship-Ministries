using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IEducationRequestService
{
    Task<List<EducationRequest>> GetAllAsync();
    Task<List<EducationRequest>> GetByUserAsync(string userId);
    Task<List<EducationRequest>> GetByStatusAsync(RequestStatus status);
    Task<EducationRequest?> GetByIdAsync(int id);
    Task CreateAsync(EducationRequest request);
    Task UpdateAsync(EducationRequest request);
    Task UpdateStatusAsync(int id, RequestStatus status, string? notes = null);
}
