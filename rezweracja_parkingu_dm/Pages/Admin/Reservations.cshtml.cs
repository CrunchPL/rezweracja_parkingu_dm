using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")] // Ochrona widoku admina
public class ReservationsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ReservationsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ReservationViewModel> Reservations { get; set; } = new();

    public void OnGet()
    {
        Reservations = _context.Reservations
        .Include(r => r.User) // Wymagane, jeœli relacja z u¿ytkownikiem jest skonfigurowana
        .Select(r => new ReservationViewModel
        {
            Sector = r.Sector,
            SpotNumber = r.SpotNumber,
            ReservationDate = r.ReservationDate,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            UserEmail = r.User.Email // Pobieranie emaila u¿ytkownika
        }).ToList();
    }
}
