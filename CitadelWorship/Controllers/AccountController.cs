using CitadelWorship.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CitadelWorship.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("login-submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginSubmit([FromForm] LoginInput input)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/account/login?error=Please fill in all required fields");
        }

        var user = await _userManager.FindByEmailAsync(input.Email);
        if (user == null || !user.IsActive)
        {
            return Redirect("/account/login?error=Invalid email or password");
        }

        var result = await _signInManager.PasswordSignInAsync(
            user, input.Password, input.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var returnUrl = input.ReturnUrl ?? "/";
            return LocalRedirect(returnUrl);
        }

        return Redirect("/account/login?error=Invalid email or password");
    }

    [HttpPost("register-submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterSubmit([FromForm] RegisterInput input)
    {
        if (!ModelState.IsValid)
        {
            return Redirect("/account/register?error=Please fill in all required fields correctly");
        }

        if (input.Password != input.ConfirmPassword)
        {
            return Redirect("/account/register?error=Passwords do not match");
        }

        var user = new ApplicationUser
        {
            UserName = input.Email,
            Email = input.Email,
            FirstName = input.FirstName,
            LastName = input.LastName,
            EmailConfirmed = true // In production, send confirmation email
        };

        var result = await _userManager.CreateAsync(user, input.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Redirect("/member/dashboard");
        }

        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        return Redirect($"/account/register?error={Uri.EscapeDataString(errors)}");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }

    [HttpPost("forgot-password-submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPasswordSubmit([FromForm] ForgotPasswordInput input)
    {
        if (!ModelState.IsValid)
        {
            return Redirect("/account/forgot-password?error=Please enter a valid email");
        }

        var user = await _userManager.FindByEmailAsync(input.Email);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // In production, send email with reset link
            // For now, redirect with a success message
        }

        // Always show success to prevent email enumeration
        return Redirect("/account/forgot-password?success=true");
    }

    [HttpPost("reset-password-submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPasswordSubmit([FromForm] ResetPasswordInput input)
    {
        if (!ModelState.IsValid)
        {
            return Redirect($"/account/reset-password?token={input.Token}&email={input.Email}&error=Please fill in all fields");
        }

        if (input.Password != input.ConfirmPassword)
        {
            return Redirect($"/account/reset-password?token={input.Token}&email={input.Email}&error=Passwords do not match");
        }

        var user = await _userManager.FindByEmailAsync(input.Email);
        if (user == null)
        {
            return Redirect("/account/login?error=Password reset failed");
        }

        var result = await _userManager.ResetPasswordAsync(user, input.Token, input.Password);
        if (result.Succeeded)
        {
            return Redirect("/account/login?success=Password reset successful. Please log in.");
        }

        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        return Redirect($"/account/reset-password?token={input.Token}&email={input.Email}&error={Uri.EscapeDataString(errors)}");
    }

    [HttpPost("update-profile")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile([FromForm] ProfileInput input)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Redirect("/account/login");

        user.FirstName = input.FirstName;
        user.LastName = input.LastName;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Redirect("/account/profile?success=Profile updated successfully");
        }

        return Redirect("/account/profile?error=Failed to update profile");
    }

    [HttpPost("change-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordInput input)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Redirect("/account/login");

        if (input.NewPassword != input.ConfirmNewPassword)
        {
            return Redirect("/account/profile?error=New passwords do not match");
        }

        var result = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user);
            return Redirect("/account/profile?success=Password changed successfully");
        }

        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        return Redirect($"/account/profile?error={Uri.EscapeDataString(errors)}");
    }
}

// Input models for form binding
public class LoginInput
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

public class RegisterInput
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ForgotPasswordInput
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordInput
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ProfileInput
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
}

public class ChangePasswordInput
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
