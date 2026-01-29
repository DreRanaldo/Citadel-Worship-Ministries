using CitadelWorship.Components;
using CitadelWorship.Data;
using CitadelWorship.Data.Models;
using CitadelWorship.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor and MVC services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Entity Framework Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = false; // Set true in production
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie configuration
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
});

// Application services
builder.Services.AddScoped<ISermonService, SermonService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IMinistryService, MinistryService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IPrayerRequestService, PrayerRequestService>();
builder.Services.AddScoped<ISiteSettingsService, SiteSettingsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMemberProfileService, MemberProfileService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IFinancialService, FinancialService>();
builder.Services.AddScoped<IEducationRequestService, EducationRequestService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<ISpecialNoteService, SpecialNoteService>();
builder.Services.AddScoped<ILeaderService, LeaderService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();

// Authorization policies
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireAdminAssistant", policy => policy.RequireRole("Admin", "AdminAssistant"));
    options.AddPolicy("RequireMember", policy => policy.RequireRole("Member", "Admin", "AdminAssistant"));
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services, builder.Configuration);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
