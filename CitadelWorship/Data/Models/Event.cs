using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public class Event
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string? Description { get; set; }

    [MaxLength(300)]
    public string? Location { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsMembersOnly { get; set; } = false;

    public bool IsPublished { get; set; } = true;

    [MaxLength(200)]
    public string? ContactEmail { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
