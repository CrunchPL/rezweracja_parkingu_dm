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
            .Join(_context.Users, // Po³¹czenie tabel
                  r => r.UserId,
                  u => u.Id,
                  (r, u) => new ReservationViewModel
                  {
                      Sector = r.Sector,
                      SpotNumber = r.SpotNumber,
                      ReservationDate = r.ReservationDate,
                      StartTime = r.StartTime,
                      EndTime = r.EndTime,
                      UserEmail = u.Email // Pobieranie adresu e-mail u¿ytkownika
                  })
            .ToList();
    }




}
