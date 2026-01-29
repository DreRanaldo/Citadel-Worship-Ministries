using CitadelWorship.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sermon> Sermons => Set<Sermon>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Ministry> Ministries => Set<Ministry>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<PrayerRequest> PrayerRequests => Set<PrayerRequest>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<FinancialRecord> FinancialRecords => Set<FinancialRecord>();
    public DbSet<EducationRequest> EducationRequests => Set<EducationRequest>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<SpecialNote> SpecialNotes => Set<SpecialNote>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PrayerRequest>()
            .HasOne(p => p.SubmittedByUser)
            .WithMany(u => u.PrayerRequests)
            .HasForeignKey(p => p.SubmittedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Sermon>()
            .HasIndex(s => s.DatePreached);

        builder.Entity<Event>()
            .HasIndex(e => e.StartDate);

        builder.Entity<Announcement>()
            .HasIndex(a => a.PublishDate);

        builder.Entity<MemberProfile>()
            .HasOne(m => m.User)
            .WithOne()
            .HasForeignKey<MemberProfile>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Attendance>()
            .HasIndex(a => a.ServiceDate);

        builder.Entity<FinancialRecord>()
            .HasIndex(f => f.DateRecorded);

        builder.Entity<FinancialRecord>()
            .HasIndex(f => f.RecordType);

        builder.Entity<EducationRequest>()
            .HasIndex(e => e.Status);

        builder.Entity<AuditLog>()
            .HasIndex(a => a.Timestamp);

        builder.Entity<AuditLog>()
            .HasIndex(a => a.Action);

        builder.Entity<SpecialNote>()
            .HasIndex(n => n.Category);
    }
}
