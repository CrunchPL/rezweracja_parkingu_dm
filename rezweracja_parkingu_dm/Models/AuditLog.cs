using System;

namespace rezweracja_parkingu_dm.Models
{
    public class AuditLog
    {
        public int Id { get; set; } // Unikalny identyfikator logu
        public string UserId { get; set; } // ID użytkownika
        public string Action { get; set; } // Działanie (np. Logowanie, Rejestracja)
        public string Details { get; set; } // Szczegóły logu
        public DateTime Timestamp { get; set; } // Czas zdarzenia
    }
}
