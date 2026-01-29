using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public class Leader
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty; // e.g., "Senior Pastor", "Worship Leader"

    [MaxLength(2000)]
    public string? Bio { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
