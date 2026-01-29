using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public interface IAttendanceService
{
    Task<List<Attendance>> GetAllAsync();
    Task<List<Attendance>> GetByDateAsync(DateTime date);
    Task<List<Attendance>> GetByServiceTypeAsync(string serviceType);
    Task<List<Attendance>> GetByDateRangeAsync(DateTime start, DateTime end);
    Task<List<Attendance>> GetByMemberAsync(string memberId);
    Task<Attendance?> GetByIdAsync(int id);
    Task CreateAsync(Attendance attendance);
    Task CreateBatchAsync(List<Attendance> records);
    Task<Dictionary<string, int>> GetAttendanceSummaryAsync(DateTime start, DateTime end);
}
