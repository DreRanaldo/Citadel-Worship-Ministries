using CitadelWorship.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CitadelWorship.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.EnsureCreatedAsync();

        // Seed roles
        string[] roles = { "Admin", "AdminAssistant", "Member" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed admin user
        var adminEmail = configuration["SeedAdmin:Email"] ?? "admin@citadelworship.org";
        var adminPassword = configuration["SeedAdmin:Password"] ?? "Admin@123456";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Church",
                LastName = "Admin",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed admin assistant user
        var assistantEmail = "assistant@citadelworship.org";
        if (await userManager.FindByEmailAsync(assistantEmail) == null)
        {
            var assistantUser = new ApplicationUser
            {
                UserName = assistantEmail,
                Email = assistantEmail,
                FirstName = "Mary",
                LastName = "Assistant",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(assistantUser, "Assistant@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(assistantUser, "AdminAssistant");
            }
        }

        // Seed sample member
        var memberEmail = "member@citadelworship.org";
        if (await userManager.FindByEmailAsync(memberEmail) == null)
        {
            var memberUser = new ApplicationUser
            {
                UserName = memberEmail,
                Email = memberEmail,
                FirstName = "John",
                LastName = "Member",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(memberUser, "Member@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(memberUser, "Member");
            }
        }

        // Seed site settings
        if (!await context.SiteSettings.AnyAsync())
        {
            context.SiteSettings.Add(new SiteSettings
            {
                ChurchName = "Citadel Worship Ministries",
                Tagline = "A Place of Worship, Community, and Transformation",
                HeroScripture = "\"For where two or three gather in my name, there am I with them.\" — Matthew 18:20",
                Address = "123 Faith Avenue, Springfield, IL 62701",
                Phone = "(555) 123-4567",
                Email = "info@citadelworship.org",
                SundayServiceTimes = "9:00 AM & 11:00 AM",
                WednesdayServiceTimes = "7:00 PM - Bible Study",
                FridayServiceTimes = "7:00 PM - Prayer Night",
                FacebookUrl = "https://facebook.com/citadelworship",
                YouTubeUrl = "https://youtube.com/@citadelworship",
                InstagramUrl = "https://instagram.com/citadelworship"
            });
        }

        // Seed sample sermons
        if (!await context.Sermons.AnyAsync())
        {
            context.Sermons.AddRange(
                new Sermon
                {
                    Title = "Walking in Faith",
                    Speaker = "Pastor David Thompson",
                    Description = "An inspiring message about trusting God through every season of life. Learn how Abraham's faith journey mirrors our own walk with the Lord.",
                    ScriptureReference = "Hebrews 11:1-6",
                    DatePreached = DateTime.UtcNow.AddDays(-7),
                    Series = "Foundations of Faith",
                    VideoUrl = "https://www.youtube.com/embed/example1",
                    IsPublished = true
                },
                new Sermon
                {
                    Title = "The Power of Prayer",
                    Speaker = "Pastor David Thompson",
                    Description = "Discover the transformative power of prayer and how it strengthens our relationship with God. This sermon explores biblical examples of answered prayers.",
                    ScriptureReference = "Philippians 4:6-7",
                    DatePreached = DateTime.UtcNow.AddDays(-14),
                    Series = "Foundations of Faith",
                    VideoUrl = "https://www.youtube.com/embed/example2",
                    IsPublished = true
                },
                new Sermon
                {
                    Title = "Love One Another",
                    Speaker = "Minister Sarah Johnson",
                    Description = "A heartfelt message about the greatest commandment and how we can demonstrate God's love in our daily interactions with others.",
                    ScriptureReference = "John 13:34-35",
                    DatePreached = DateTime.UtcNow.AddDays(-21),
                    Series = "Living the Gospel",
                    IsPublished = true
                },
                new Sermon
                {
                    Title = "Renewing Your Mind",
                    Speaker = "Pastor David Thompson",
                    Description = "Understanding the importance of spiritual renewal and how meditating on God's Word transforms our thinking and our lives.",
                    ScriptureReference = "Romans 12:1-2",
                    DatePreached = DateTime.UtcNow.AddDays(-28),
                    Series = "Living the Gospel",
                    IsPublished = true
                }
            );
        }

        // Seed sample events
        if (!await context.Events.AnyAsync())
        {
            context.Events.AddRange(
                new Event
                {
                    Title = "Sunday Worship Service",
                    Description = "Join us for a powerful time of worship, prayer, and the Word. All are welcome!",
                    Location = "Main Sanctuary",
                    StartDate = DateTime.UtcNow.AddDays(GetDaysUntilNext(DayOfWeek.Sunday)),
                    Category = "Worship",
                    IsPublished = true
                },
                new Event
                {
                    Title = "Community Outreach Day",
                    Description = "We're heading out into the community to serve and share God's love. Volunteers needed for food distribution, prayer walks, and neighborhood cleanup.",
                    Location = "Meet at Church Parking Lot",
                    StartDate = DateTime.UtcNow.AddDays(14),
                    EndDate = DateTime.UtcNow.AddDays(14).AddHours(4),
                    Category = "Outreach",
                    ContactEmail = "outreach@citadelworship.org",
                    IsPublished = true
                },
                new Event
                {
                    Title = "Youth Night",
                    Description = "A special evening for our young people with games, worship, and a relevant message. Ages 13-18 welcome.",
                    Location = "Youth Center",
                    StartDate = DateTime.UtcNow.AddDays(GetDaysUntilNext(DayOfWeek.Friday)),
                    Category = "Youth",
                    IsPublished = true
                },
                new Event
                {
                    Title = "Women's Bible Study",
                    Description = "A weekly gathering for women to study the Word together, share testimonies, and build lasting friendships.",
                    Location = "Fellowship Hall",
                    StartDate = DateTime.UtcNow.AddDays(GetDaysUntilNext(DayOfWeek.Tuesday)),
                    Category = "Study",
                    IsPublished = true
                },
                new Event
                {
                    Title = "Members-Only Fellowship Dinner",
                    Description = "A special dinner for church members to connect, fellowship, and discuss upcoming ministry plans.",
                    Location = "Fellowship Hall",
                    StartDate = DateTime.UtcNow.AddDays(21),
                    Category = "Fellowship",
                    IsMembersOnly = true,
                    IsPublished = true
                }
            );
        }

        // Seed sample ministries
        if (!await context.Ministries.AnyAsync())
        {
            context.Ministries.AddRange(
                new Ministry
                {
                    Name = "Worship Ministry",
                    Description = "Our worship team leads the congregation in praise and worship every Sunday. We welcome singers, musicians, and sound technicians who want to use their gifts for God's glory.",
                    LeaderName = "Minister Sarah Johnson",
                    LeaderEmail = "worship@citadelworship.org",
                    MeetingSchedule = "Thursdays at 7:00 PM (Rehearsal)",
                    MeetingLocation = "Main Sanctuary",
                    SortOrder = 1
                },
                new Ministry
                {
                    Name = "Youth Ministry",
                    Description = "Reaching the next generation with the Gospel. Our youth ministry provides a safe and exciting environment for teenagers to grow in their faith through relevant teaching, mentorship, and fun activities.",
                    LeaderName = "Brother Marcus Williams",
                    LeaderEmail = "youth@citadelworship.org",
                    MeetingSchedule = "Fridays at 6:30 PM",
                    MeetingLocation = "Youth Center",
                    SortOrder = 2
                },
                new Ministry
                {
                    Name = "Children's Ministry",
                    Description = "We believe in teaching children about Jesus in creative and age-appropriate ways. Our children's church runs during the main Sunday service for ages 3-12.",
                    LeaderName = "Sister Grace Okonkwo",
                    LeaderEmail = "children@citadelworship.org",
                    MeetingSchedule = "Sundays during service",
                    MeetingLocation = "Children's Wing",
                    SortOrder = 3
                },
                new Ministry
                {
                    Name = "Outreach & Missions",
                    Description = "Serving our local community and beyond through food drives, community events, and mission trips. We are the hands and feet of Jesus.",
                    LeaderName = "Deacon Robert Chen",
                    LeaderEmail = "outreach@citadelworship.org",
                    MeetingSchedule = "Second Saturday of each month",
                    MeetingLocation = "Fellowship Hall",
                    SortOrder = 4
                },
                new Ministry
                {
                    Name = "Prayer Ministry",
                    Description = "A dedicated team of intercessors who pray for the church, the community, and prayer requests submitted by members. We believe in the power of corporate prayer.",
                    LeaderName = "Mother Patricia Davis",
                    LeaderEmail = "prayer@citadelworship.org",
                    MeetingSchedule = "Daily at 6:00 AM (Virtual) | Fridays at 7:00 PM",
                    MeetingLocation = "Prayer Room / Online",
                    SortOrder = 5
                },
                new Ministry
                {
                    Name = "Men's Fellowship",
                    Description = "Building strong men of God through Bible study, accountability groups, and community service. Open to all men in the church.",
                    LeaderName = "Elder James Wright",
                    LeaderEmail = "men@citadelworship.org",
                    MeetingSchedule = "First and Third Saturdays at 8:00 AM",
                    MeetingLocation = "Fellowship Hall",
                    SortOrder = 6
                }
            );
        }

        // Seed sample announcements
        if (!await context.Announcements.AnyAsync())
        {
            context.Announcements.AddRange(
                new Announcement
                {
                    Title = "Welcome to Our New Website!",
                    Content = "We are excited to launch our new church website! Here you can find information about our services, ministries, upcoming events, and more. Create an account to access member-only features.",
                    PublishDate = DateTime.UtcNow,
                    IsPublished = true,
                    Category = "General"
                },
                new Announcement
                {
                    Title = "Annual Church Retreat Registration Open",
                    Content = "Registration is now open for our annual church retreat. This year's theme is 'Renewed in His Presence.' Early bird pricing available until the end of the month. See the events page for details.",
                    PublishDate = DateTime.UtcNow.AddDays(-3),
                    ExpiryDate = DateTime.UtcNow.AddDays(30),
                    IsPublished = true,
                    Category = "Events"
                },
                new Announcement
                {
                    Title = "Members Meeting - Important Update",
                    Content = "There will be a special members-only meeting next Sunday after the second service to discuss upcoming building renovations and ministry expansion plans. Your attendance is important.",
                    PublishDate = DateTime.UtcNow.AddDays(-1),
                    IsMembersOnly = true,
                    IsPublished = true,
                    Category = "Church Business"
                }
            );
        }

        await context.SaveChangesAsync();
    }

    private static int GetDaysUntilNext(DayOfWeek target)
    {
        var today = DateTime.UtcNow.DayOfWeek;
        int daysUntil = ((int)target - (int)today + 7) % 7;
        return daysUntil == 0 ? 7 : daysUntil;
    }
}
