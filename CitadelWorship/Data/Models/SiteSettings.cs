using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Data.Models;

public class SiteSettings
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string ChurchName { get; set; } = "Citadel Worship Ministries";

    [MaxLength(500)]
    public string? Tagline { get; set; } = "A Place of Worship, Community, and Transformation";

    [MaxLength(1000)]
    public string? HeroScripture { get; set; } = "\"For where two or three gather in my name, there am I with them.\" — Matthew 18:20";

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? SundayServiceTimes { get; set; } = "9:00 AM & 11:00 AM";

    [MaxLength(500)]
    public string? WednesdayServiceTimes { get; set; } = "7:00 PM";

    [MaxLength(500)]
    public string? FridayServiceTimes { get; set; }

    [MaxLength(500)]
    public string? FacebookUrl { get; set; }

    [MaxLength(500)]
    public string? YouTubeUrl { get; set; }

    [MaxLength(500)]
    public string? InstagramUrl { get; set; }

    [MaxLength(500)]
    public string? LiveStreamUrl { get; set; }

    [MaxLength(500)]
    public string? DonationUrl { get; set; }

    [MaxLength(500)]
    public string? HeroImageUrl { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
