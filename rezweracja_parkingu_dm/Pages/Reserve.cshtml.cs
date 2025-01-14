using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using rezweracja_parkingu_dm.Data;

[Authorize]
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
    public int? Id { get; set; }

    [BindProperty]
    public bool IsEdit { get; set; }

    [BindProperty]
    public string Sector { get; set; }

    [BindProperty]
    public int SpotNumber { get; set; }

    [BindProperty]
    public DateTime ReservationDate { get; set; } = DateTime.Today;

    [BindProperty]
    public TimeSpan StartTime { get; set; }

    [BindProperty]
    public TimeSpan EndTime { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id.HasValue)
        {
            var reservation = await _context.Reservations.FindAsync(id.Value);
            if (reservation == null) return NotFound();

            Id = reservation.Id;
            Sector = reservation.Sector;
            SpotNumber = reservation.SpotNumber;
            ReservationDate = reservation.ReservationDate;
            StartTime = reservation.StartTime;
            EndTime = reservation.EndTime;

            IsEdit = true;
        }
        else
        {
            ReservationDate = DateTime.Today;
            IsEdit = false;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage("/Account/Login");

        if (ReservationDate < DateTime.Today)
        {
            ModelState.AddModelError(string.Empty, "Nie mo¿na dokonaæ rezerwacji na przesz³¹ datê.");
            return Page();
        }

        if (StartTime >= EndTime)
        {
            ModelState.AddModelError(string.Empty, "Godzina zakoñczenia musi byæ póŸniejsza ni¿ godzina rozpoczêcia.");
            return Page();
        }

        var overlappingReservation = _context.Reservations
            .Where(r => r.Sector == Sector && r.SpotNumber == SpotNumber && r.ReservationDate == ReservationDate)
            .Any(r => r.Id != Id && (StartTime < r.EndTime && EndTime > r.StartTime));

        if (overlappingReservation)
        {
            ModelState.AddModelError(string.Empty, "To miejsce jest ju¿ zarezerwowane w wybranych godzinach.");
            return Page();
        }

        if (Id.HasValue)
        {
            var reservation = await _context.Reservations.FindAsync(Id.Value);
            if (reservation == null) return NotFound();

            reservation.StartTime = StartTime;
            reservation.EndTime = EndTime;

            _context.Reservations.Update(reservation);
        }
        else
        {
            var reservation = new Reservation
            {
                Sector = Sector,
                SpotNumber = SpotNumber,
                ReservationDate = ReservationDate,
                StartTime = StartTime,
                EndTime = EndTime,
                UserId = user.Id
            };

            _context.Reservations.Add(reservation);
        }

        await _context.SaveChangesAsync();
        return RedirectToPage("/MyReservations");
    }
}
