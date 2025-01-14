using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using rezweracja_parkingu_dm.Services;

namespace rezweracja_parkingu_dm.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuditLogger _auditLogger;

        public LoginModel(SignInManager<IdentityUser> signInManager, AuditLogger auditLogger)
        {
            _signInManager = signInManager;
            _auditLogger = auditLogger;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _signInManager.PasswordSignInAsync(Email, Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    Console.WriteLine($"Nie znaleziono u¿ytkownika o emailu: {Email}");
                    return Page();
                }

                Console.WriteLine($"Zalogowano u¿ytkownika: {Email} (ID: {user.Id})");

                try
                {
                    await _auditLogger.LogAsync(user.Id, "Logowanie", $"U¿ytkownik {Email} zalogowa³ siê.");
                    Console.WriteLine("Zdarzenie logowania zapisane.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"B³¹d zapisu zdarzenia logowania: {ex.Message}");
                }

                return RedirectToPage("/Index");
            }
            else
            {
                try
                {
                    await _auditLogger.LogAsync("", "Nieudane logowanie", $"Nieudana próba logowania na konto {Email}.");
                    Console.WriteLine("Zdarzenie nieudanego logowania zapisane.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"B³¹d zapisu zdarzenia nieudanego logowania: {ex.Message}");
                }

                ModelState.AddModelError(string.Empty, "Nieprawid³owy email lub has³o.");
                return Page();
            }
        }

    }
}
