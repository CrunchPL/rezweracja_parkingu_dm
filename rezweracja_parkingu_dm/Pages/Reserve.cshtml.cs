using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ReserveModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ReserveModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public string Sector { get; set; }

    [BindProperty]
    public int SpotNumber { get; set; }

    [BindProperty]
    public DateTime ReservationDate { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage("Login");

        var reservation = new Reservation
        {
            Sector = Sector,
            SpotNumber = SpotNumber,
            ReservationDate = ReservationDate,
            UserId = user.Id
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return RedirectToPage("MyReservations");
    }
}
