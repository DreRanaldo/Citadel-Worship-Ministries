# Citadel Worship Ministries Website

A modern, responsive church website built with **ASP.NET Core 8** and **Blazor Server**, featuring public pages, member access, and a full admin dashboard.

## Tech Stack

- **Framework:** ASP.NET Core 8 + Blazor Server
- **Authentication:** ASP.NET Core Identity
- **Database:** SQLite (via Entity Framework Core)
- **UI:** Bootstrap 5 + Bootstrap Icons
- **ORM:** Entity Framework Core 8

## Features

### Public Website
- **Home** - Hero section with scripture, service times, latest sermons, upcoming events, and ministry previews
- **About Us** - Church history, mission & beliefs, leadership profiles
- **Ministries** - Ministry listings with details, leaders, and schedules
- **Events** - Upcoming events with calendar-style date badges
- **Sermons** - Sermon archive with search and series filtering, video/audio embeds
- **Give** - Donation information page (mock integration)
- **Contact** - Contact form with church location and details

### Authentication & Authorization
- Login / Register / Logout
- Password reset flow (mocked email)
- Role-based access: Guest, Member, Admin
- Profile management with password change

### Member Dashboard (Requires Login)
- Personalized dashboard with quick links
- View all announcements (including members-only)
- View all events (including members-only)
- Submit and track prayer requests

### Admin Dashboard (Admin Role Required)
- Overview with content statistics
- **Sermon Management** - Full CRUD for sermons
- **Event Management** - Full CRUD for events
- **Ministry Management** - Full CRUD for ministries
- **Announcement Management** - Full CRUD with publish/expiry dates
- **User Management** - View users, assign/remove roles, enable/disable accounts
- **Prayer Requests** - View, filter by status, update status (New/Prayed/Archived)
- **Site Settings** - Edit church name, service times, contact info, social media links

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A code editor (Visual Studio 2022, VS Code, or Rider)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/DreRanaldo/Citadel-Worship-Ministries.git
cd Citadel-Worship-Ministries
```

### 2. Restore Packages

```bash
dotnet restore CitadelWorship/CitadelWorship.csproj
```

### 3. Run the Application

```bash
dotnet run --project CitadelWorship
```

The app will start and be available at:
- **HTTPS:** https://localhost:7171
- **HTTP:** http://localhost:5171

### 4. First Run

The database is automatically created and seeded on first run with:

| Account | Email | Password | Role |
|---------|-------|----------|------|
| Admin | admin@citadelworship.org | Admin@123456 | Admin |
| Member | member@citadelworship.org | Member@123456 | Member |

Sample sermons, events, ministries, and announcements are also seeded.

## Project Structure

```
CitadelWorship/
├── Components/
│   ├── App.razor                    # Root component
│   ├── Routes.razor                 # Router configuration
│   ├── _Imports.razor               # Global using directives
│   ├── Layout/
│   │   ├── MainLayout.razor         # Public site layout
│   │   ├── AdminLayout.razor        # Admin panel layout with sidebar
│   │   └── NavMenu.razor            # Navigation with auth-aware links
│   ├── Pages/
│   │   ├── Home.razor               # Homepage
│   │   ├── About.razor              # About us
│   │   ├── Ministries.razor         # Ministry list
│   │   ├── MinistryDetails.razor    # Individual ministry page
│   │   ├── Events.razor             # Events list
│   │   ├── EventDetails.razor       # Individual event page
│   │   ├── Sermons.razor            # Sermon archive with search
│   │   ├── SermonDetails.razor      # Individual sermon with media
│   │   ├── Give.razor               # Donation page
│   │   ├── Contact.razor            # Contact form
│   │   ├── Error.razor              # Error page
│   │   ├── Account/                 # Auth pages (Login, Register, etc.)
│   │   ├── Member/                  # Member-only pages
│   │   └── Admin/                   # Admin CRUD pages
│   └── Shared/
│       ├── Footer.razor             # Site footer
│       └── RedirectToLogin.razor    # Auth redirect helper
├── Controllers/
│   └── AccountController.cs         # Auth endpoints (login, register, logout)
├── Data/
│   ├── ApplicationDbContext.cs       # EF Core DbContext
│   ├── SeedData.cs                  # Database seeder
│   └── Models/
│       ├── ApplicationUser.cs       # Extended Identity user
│       ├── Sermon.cs
│       ├── Event.cs
│       ├── Ministry.cs
│       ├── Announcement.cs
│       ├── PrayerRequest.cs
│       ├── SiteSettings.cs
│       └── ContactMessage.cs
├── Services/                        # Business logic layer (interfaces + implementations)
├── wwwroot/
│   └── css/app.css                  # Custom styles
├── Program.cs                       # Application entry point
├── appsettings.json                 # Configuration
└── CitadelWorship.csproj            # Project file
```

## EF Core Migrations

To create and apply migrations:

```bash
cd CitadelWorship

# Add a migration
dotnet ef migrations add InitialCreate

# Apply migrations
dotnet ef database update
```

> Note: The app uses `EnsureCreated()` by default for quick setup. For production, switch to explicit migrations.

## Configuration

### Database

The default connection uses SQLite. To switch to SQL Server, update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CitadelWorship;Trusted_Connection=True;"
  }
}
```

And update `Program.cs` to use `UseSqlServer()` instead of `UseSqlite()`.

### Default Admin Credentials

Configure in `appsettings.json`:

```json
{
  "SeedAdmin": {
    "Email": "admin@citadelworship.org",
    "Password": "Admin@123456"
  }
}
```

> Change these credentials before deploying to production.

## Switching to Blazor WebAssembly

To convert to Blazor WASM hosting:

1. Create a separate client project targeting `Microsoft.NET.Sdk.BlazorWebAssembly`
2. Move components to the client project
3. Create API controllers for data access
4. Update `Program.cs` to serve the WASM client
5. Replace service implementations with `HttpClient`-based calls

## Security Notes

- Role-based route protection via `[Authorize]` attributes
- Server-side validation on all forms
- Anti-forgery protection on auth forms
- Password hashing via ASP.NET Core Identity
- Cookie-based authentication with secure defaults

## License

This project is for the use of Citadel Worship Ministries.
