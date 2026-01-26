using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public class Ministry
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? LeaderName { get; set; }

    [MaxLength(200)]
    public string? LeaderEmail { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(200)]
    public string? MeetingSchedule { get; set; }

    [MaxLength(300)]
    public string? MeetingLocation { get; set; }

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
