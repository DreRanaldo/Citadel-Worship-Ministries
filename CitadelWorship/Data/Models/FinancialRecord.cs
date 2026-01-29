using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitadelWorship.Data.Models;

public enum FinancialRecordType
{
    Tithe,
    Offering,
    SpecialSeed
}

public enum PaymentMethod
{
    Cash,
    Transfer,
    Cheque,
    Online,
    Other
}

public class FinancialRecord
{
    public int Id { get; set; }

    [Required]
    public FinancialRecordType RecordType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime DateRecorded { get; set; }

    public string? MemberId { get; set; }

    [ForeignKey("MemberId")]
    public ApplicationUser? Member { get; set; }

    [MaxLength(200)]
    public string? MemberName { get; set; } // For anonymous or non-member giving

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [MaxLength(200)]
    public string? ServiceOrEvent { get; set; }

    [MaxLength(200)]
    public string? Category { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    // Audit - cannot be edited after submission
    [Required]
    public string SubmittedByUserId { get; set; } = string.Empty;

    [ForeignKey("SubmittedByUserId")]
    public ApplicationUser? SubmittedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Immutable after creation - no UpdatedAt
}
