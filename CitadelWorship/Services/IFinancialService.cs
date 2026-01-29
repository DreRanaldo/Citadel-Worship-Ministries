using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public class FinancialSummary
{
    public decimal TotalTithes { get; set; }
    public decimal TotalOfferings { get; set; }
    public decimal TotalSpecialSeeds { get; set; }
    public decimal GrandTotal => TotalTithes + TotalOfferings + TotalSpecialSeeds;
    public int TotalRecords { get; set; }
}

public interface IFinancialService
{
    Task<List<FinancialRecord>> GetAllAsync();
    Task<List<FinancialRecord>> GetByTypeAsync(FinancialRecordType type);
    Task<List<FinancialRecord>> GetByDateRangeAsync(DateTime start, DateTime end);
    Task<FinancialRecord?> GetByIdAsync(int id);
    Task CreateAsync(FinancialRecord record);
    Task<FinancialSummary> GetSummaryAsync(DateTime? start = null, DateTime? end = null);
    Task<List<FinancialRecord>> GetByMemberAsync(string memberId);
}
