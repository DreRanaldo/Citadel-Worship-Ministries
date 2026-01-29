using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitadelWorship.Data.Models;

public enum EducationRequestType
{
    HomeworkHelp,
    PrintingAssistance,
    EducationalSupport,
    Other
}

public enum RequestStatus
{
    Submitted,
    InReview,
    Assigned,
    Completed,
    Cancelled
}

public class EducationRequest
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public EducationRequestType RequestType { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Submitted;

    public string? SubmittedByUserId { get; set; }

    [ForeignKey("SubmittedByUserId")]
    public ApplicationUser? SubmittedByUser { get; set; }

    [MaxLength(200)]
    public string? AssignedTo { get; set; }

    [MaxLength(2000)]
    public string? InternalNotes { get; set; }

    [MaxLength(2000)]
    public string? ResolutionNotes { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
