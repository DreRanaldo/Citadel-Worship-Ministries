using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitadelWorship.Data.Models;

public class GalleryImage
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required, MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; } // e.g., "Worship", "Events", "Community", "Youth"

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; } = 0;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public string? UploadedByUserId { get; set; }

    [ForeignKey("UploadedByUserId")]
    public ApplicationUser? UploadedByUser { get; set; }
}
