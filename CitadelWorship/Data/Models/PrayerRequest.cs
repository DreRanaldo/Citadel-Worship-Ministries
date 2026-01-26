using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public enum PrayerRequestStatus
{
    New,
    Prayed,
    Archived
}

public class PrayerRequest
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public PrayerRequestStatus Status { get; set; } = PrayerRequestStatus.New;

    public bool IsAnonymous { get; set; } = false;

    public string? SubmittedByUserId { get; set; }

    public ApplicationUser? SubmittedByUser { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
