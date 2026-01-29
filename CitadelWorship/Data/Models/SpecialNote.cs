using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitadelWorship.Data.Models;

public enum NoteCategory
{
    FirstTimeVisitor,
    CounselingReferral,
    FollowUpRequired,
    General,
    Confidential
}

public class SpecialNote
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required, MaxLength(4000)]
    public string Content { get; set; } = string.Empty;

    public NoteCategory Category { get; set; } = NoteCategory.General;

    public string? RelatedMemberId { get; set; }

    [ForeignKey("RelatedMemberId")]
    public ApplicationUser? RelatedMember { get; set; }

    [MaxLength(200)]
    public string? RelatedMemberName { get; set; }

    public bool IsConfidential { get; set; } = false;

    public string? CreatedByUserId { get; set; }

    [ForeignKey("CreatedByUserId")]
    public ApplicationUser? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
