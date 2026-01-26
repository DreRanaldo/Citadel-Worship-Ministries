using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ProfileImageUrl { get; set; }

    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public string FullName => $"{FirstName} {LastName}".Trim();

    // Navigation properties
    public ICollection<PrayerRequest> PrayerRequests { get; set; } = new List<PrayerRequest>();
}
