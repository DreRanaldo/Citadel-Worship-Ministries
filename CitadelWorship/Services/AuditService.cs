using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;
    public AuditService(ApplicationDbContext context) => _context = context;

    public async Task LogAsync(string action, string entityType, string? entityId = null, string? details = null, string? userId = null, string? userName = null)
    {
        _context.AuditLogs.Add(new AuditLog
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            UserId = userId,
            UserName = userName,
            Timestamp = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync(int limit = 100) =>
        await _context.AuditLogs.OrderByDescending(a => a.Timestamp).Take(limit).ToListAsync();

    public async Task<List<AuditLog>> GetByEntityAsync(string entityType, string? entityId = null)
    {
        var query = _context.AuditLogs.Where(a => a.EntityType == entityType);
        if (entityId != null) query = query.Where(a => a.EntityId == entityId);
        return await query.OrderByDescending(a => a.Timestamp).ToListAsync();
    }

    public async Task<List<AuditLog>> GetByUserAsync(string userId) =>
        await _context.AuditLogs.Where(a => a.UserId == userId).OrderByDescending(a => a.Timestamp).ToListAsync();

    public async Task<List<AuditLog>> GetByDateRangeAsync(DateTime start, DateTime end) =>
        await _context.AuditLogs.Where(a => a.Timestamp >= start && a.Timestamp <= end).OrderByDescending(a => a.Timestamp).ToListAsync();
}
