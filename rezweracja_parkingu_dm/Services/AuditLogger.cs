using rezweracja_parkingu_dm.Data;
using rezweracja_parkingu_dm.Models;
using System.Threading.Tasks;

namespace rezweracja_parkingu_dm.Services
{
    public class AuditLogger
    {
        private readonly ApplicationDbContext _context;

        public AuditLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string userId, string action, string details)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            Console.WriteLine($"Dodawanie logu: {log.UserId}, {log.Action}, {log.Details}, {log.Timestamp}");

            _context.AuditLogs.Add(log);
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("Log zapisany w bazie danych.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd zapisu logu: {ex.Message}");
            }
        }

    }
}


