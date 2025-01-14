using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using rezweracja_parkingu_dm.Services; // Dodaj przestrzeñ nazw dla AuditLogger

public class RegisterModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AuditLogger _auditLogger;

    public RegisterModel(UserManager<IdentityUser> userManager, AuditLogger auditLogger)
    {
        _userManager = userManager;
        _auditLogger = auditLogger; // Wstrzykniêcie AuditLogger
    }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = new IdentityUser { UserName = Email, Email = Email };
        var result = await _userManager.CreateAsync(user, Password);
        if (result.Succeeded)
        {
            // Logowanie rejestracji
            await _auditLogger.LogAsync(user.Id, "Rejestracja", "Nowe konto zosta³o zarejestrowane.");

            return RedirectToPage("Login");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
