using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Admin")]
public class UserManagementModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserManagementModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public List<UserViewModel> Users { get; set; } = new();

    public async Task OnGetAsync()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            Users.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.Any() ? string.Join(", ", roles) : "U¿ytkownik" // Wyœwietl rolê lub "U¿ytkownik"
            });
        }
    }

    public async Task<IActionResult> OnPostChangeRoleAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Admin"))
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostConfirmResetPasswordAsync(string userId, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Has³a nie s¹ zgodne.");
            return Page();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        await _userManager.DeleteAsync(user);
        return RedirectToPage();
    }
}

public class UserViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Roles { get; set; }
}
