using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using rezweracja_parkingu_dm.Models;

namespace rezweracja_parkingu_dm.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet dla istniejących tabel
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }

        // DbSet dla tabeli logów
        public DbSet<AuditLog> AuditLogs { get; set; }

        public override int SaveChanges()
        {
            LogAuditData();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            LogAuditData();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void LogAuditData()
        {
            // Tworzymy kopię kolekcji, aby uniknąć problemu z modyfikacją w trakcie iteracji
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .ToList();

            foreach (var entry in entries)
            {
                var auditLog = new AuditLog
                {
                    UserId = entry.Entity?.GetType().GetProperty("Id")?.GetValue(entry.Entity)?.ToString() ?? "Unknown",
                    Action = entry.State == EntityState.Added ? "Add" : "Update",
                    Details = entry.Entity?.ToString(),
                    Timestamp = DateTime.UtcNow
                };

                AuditLogs.Add(auditLog);
            }
        }

    }
}
