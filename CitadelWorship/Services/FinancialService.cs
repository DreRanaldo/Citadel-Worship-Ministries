using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Services;

public class FinancialService : IFinancialService
{
    private readonly ApplicationDbContext _context;
    public FinancialService(ApplicationDbContext context) => _context = context;

    public async Task<List<FinancialRecord>> GetAllAsync() =>
        await _context.FinancialRecords.Include(f => f.Member).OrderByDescending(f => f.DateRecorded).ToListAsync();

    public async Task<List<FinancialRecord>> GetByTypeAsync(FinancialRecordType type) =>
        await _context.FinancialRecords.Include(f => f.Member)
            .Where(f => f.RecordType == type).OrderByDescending(f => f.DateRecorded).ToListAsync();

    public async Task<List<FinancialRecord>> GetByDateRangeAsync(DateTime start, DateTime end) =>
        await _context.FinancialRecords.Include(f => f.Member)
            .Where(f => f.DateRecorded >= start && f.DateRecorded <= end).OrderByDescending(f => f.DateRecorded).ToListAsync();

    public async Task<FinancialRecord?> GetByIdAsync(int id) =>
        await _context.FinancialRecords.Include(f => f.Member).FirstOrDefaultAsync(f => f.Id == id);

    public async Task CreateAsync(FinancialRecord record)
    {
        record.CreatedAt = DateTime.UtcNow;
        _context.FinancialRecords.Add(record);
        await _context.SaveChangesAsync();
    }

    public async Task<FinancialSummary> GetSummaryAsync(DateTime? start = null, DateTime? end = null)
    {
        var query = _context.FinancialRecords.AsQueryable();
        if (start.HasValue) query = query.Where(f => f.DateRecorded >= start.Value);
        if (end.HasValue) query = query.Where(f => f.DateRecorded <= end.Value);

        var records = await query.ToListAsync();
        return new FinancialSummary
        {
            TotalTithes = records.Where(r => r.RecordType == FinancialRecordType.Tithe).Sum(r => r.Amount),
            TotalOfferings = records.Where(r => r.RecordType == FinancialRecordType.Offering).Sum(r => r.Amount),
            TotalSpecialSeeds = records.Where(r => r.RecordType == FinancialRecordType.SpecialSeed).Sum(r => r.Amount),
            TotalRecords = records.Count
        };
    }

    public async Task<List<FinancialRecord>> GetByMemberAsync(string memberId) =>
        await _context.FinancialRecords.Where(f => f.MemberId == memberId).OrderByDescending(f => f.DateRecorded).ToListAsync();
}
