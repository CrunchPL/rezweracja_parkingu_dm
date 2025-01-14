using Microsoft.AspNetCore.Mvc.RazorPages;
using rezweracja_parkingu_dm.Data;
using rezweracja_parkingu_dm.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rezweracja_parkingu_dm.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class LogsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public LogsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<AuditLog> Logs { get; set; }

    public async Task OnGetAsync()
    {
        Logs = _context.AuditLogs
            .OrderByDescending(log => log.Timestamp)
            .ToList();
    }
}
