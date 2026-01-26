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
    }
}
