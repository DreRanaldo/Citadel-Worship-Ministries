using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public class Sermon
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Speaker { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? ScriptureReference { get; set; }

    public DateTime DatePreached { get; set; }

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    [MaxLength(500)]
    public string? AudioUrl { get; set; }

    [MaxLength(500)]
    public string? ThumbnailUrl { get; set; }

    [MaxLength(100)]
    public string? Series { get; set; }

    public bool IsPublished { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
