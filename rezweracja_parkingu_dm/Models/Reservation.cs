using Microsoft.AspNetCore.Identity;

public class Reservation
{
    public int Id { get; set; } // Klucz główny
    public string Sector { get; set; } = string.Empty; // Sektor (np. A, B, C, D)
    public int SpotNumber { get; set; } // Numer miejsca (np. 0-9)
    public DateTime ReservationDate { get; set; } // Data rezerwacji

    public string UserId { get; set; } = string.Empty; // Identyfikator użytkownika (połączony z ASP.NET Identity)
    public IdentityUser User { get; set; } = null!; // Powiązanie z tabelą użytkowników
}
