using Microsoft.AspNetCore.Identity;

public class Reservation
{
    public int Id { get; set; }
    public string Sector { get; set; }
    public int SpotNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; } // Relacja do użytkownika
}
