using CitadelWorship.Data.Models;

namespace CitadelWorship.Services;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime DateJoined { get; set; }
    public List<string> Roles { get; set; } = new();
}

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string id);
    Task<bool> AssignRoleAsync(string userId, string role);
    Task<bool> RemoveRoleAsync(string userId, string role);
    Task<bool> ToggleUserActiveAsync(string userId);
}
