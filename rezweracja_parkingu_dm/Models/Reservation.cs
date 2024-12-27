using System;
using Microsoft.AspNetCore.Identity;

public class Reservation
{
    public int Id { get; set; } // Klucz główny
    public string Sector { get; set; } = string.Empty; // Sektor parkingu
    public int SpotNumber { get; set; } // Numer miejsca
    public DateTime ReservationDate { get; set; } // Data rezerwacji
    public TimeSpan StartTime { get; set; } // Godzina rozpoczęcia rezerwacji
    public TimeSpan EndTime { get; set; } // Godzina zakończenia rezerwacji

    public string UserId { get; set; } = string.Empty; // Id użytkownika
    public IdentityUser User { get; set; } = null!; // Powiązanie z tabelą użytkowników
}
