using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitadelWorship.Data.Models;

public class AuditLog
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Action { get; set; } = string.Empty; // Login, Create, Update, Delete, RoleChange

    [Required, MaxLength(200)]
    public string EntityType { get; set; } = string.Empty; // e.g., "FinancialRecord", "User"

    [MaxLength(200)]
    public string? EntityId { get; set; }

    [MaxLength(2000)]
    public string? Details { get; set; }

    public string? UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    [MaxLength(200)]
    public string? UserName { get; set; }

    [MaxLength(50)]
    public string? IpAddress { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
