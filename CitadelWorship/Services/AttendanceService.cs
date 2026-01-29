using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class AttendanceService : IAttendanceService
{
    private readonly ApplicationDbContext _context;
    public AttendanceService(ApplicationDbContext context) => _context = context;

    public async Task<List<Attendance>> GetAllAsync() =>
        await _context.Attendances.Include(a => a.Member).OrderByDescending(a => a.ServiceDate).ToListAsync();

    public async Task<List<Attendance>> GetByDateAsync(DateTime date) =>
        await _context.Attendances.Include(a => a.Member)
            .Where(a => a.ServiceDate.Date == date.Date).OrderBy(a => a.MemberName).ToListAsync();

    public async Task<List<Attendance>> GetByServiceTypeAsync(string serviceType) =>
        await _context.Attendances.Include(a => a.Member)
            .Where(a => a.ServiceType == serviceType).OrderByDescending(a => a.ServiceDate).ToListAsync();

    public async Task<List<Attendance>> GetByDateRangeAsync(DateTime start, DateTime end) =>
        await _context.Attendances.Include(a => a.Member)
            .Where(a => a.ServiceDate >= start && a.ServiceDate <= end).OrderByDescending(a => a.ServiceDate).ToListAsync();

    public async Task<List<Attendance>> GetByMemberAsync(string memberId) =>
        await _context.Attendances.Where(a => a.MemberId == memberId).OrderByDescending(a => a.ServiceDate).ToListAsync();

    public async Task<Attendance?> GetByIdAsync(int id) =>
        await _context.Attendances.Include(a => a.Member).FirstOrDefaultAsync(a => a.Id == id);

    public async Task CreateAsync(Attendance attendance)
    {
        attendance.CreatedAt = DateTime.UtcNow;
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
    }

    public async Task CreateBatchAsync(List<Attendance> records)
    {
        foreach (var r in records) r.CreatedAt = DateTime.UtcNow;
        _context.Attendances.AddRange(records);
        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<string, int>> GetAttendanceSummaryAsync(DateTime start, DateTime end)
    {
        return await _context.Attendances
            .Where(a => a.ServiceDate >= start && a.ServiceDate <= end && a.IsPresent)
            .GroupBy(a => a.ServiceType)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }
}
