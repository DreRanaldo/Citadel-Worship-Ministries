using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitadelWorship.Data.Models;

public class Attendance
{
    public int Id { get; set; }

    [Required]
    public DateTime ServiceDate { get; set; }

    [Required, MaxLength(100)]
    public string ServiceType { get; set; } = string.Empty; // Sunday, Bible Study, Special Service

    public string? MemberId { get; set; }

    [ForeignKey("MemberId")]
    public ApplicationUser? Member { get; set; }

    [MaxLength(200)]
    public string? MemberName { get; set; }

    public bool IsPresent { get; set; } = true;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public string? RecordedByUserId { get; set; }

    [ForeignKey("RecordedByUserId")]
    public ApplicationUser? RecordedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
