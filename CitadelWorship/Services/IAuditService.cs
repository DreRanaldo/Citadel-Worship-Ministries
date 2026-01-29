using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IAuditService
{
    Task LogAsync(string action, string entityType, string? entityId = null, string? details = null, string? userId = null, string? userName = null);
    Task<List<AuditLog>> GetAllAsync(int limit = 100);
    Task<List<AuditLog>> GetByEntityAsync(string entityType, string? entityId = null);
    Task<List<AuditLog>> GetByUserAsync(string userId);
    Task<List<AuditLog>> GetByDateRangeAsync(DateTime start, DateTime end);
}
