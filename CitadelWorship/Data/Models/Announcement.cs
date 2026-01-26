using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public class Announcement
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiryDate { get; set; }

    public bool IsMembersOnly { get; set; } = false;

    public bool IsPublished { get; set; } = true;

    [MaxLength(100)]
    public string? Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
