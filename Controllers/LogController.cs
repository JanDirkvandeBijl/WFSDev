using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFSDev.Data;

namespace WFSDev.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private readonly ProductionDbContext _context;

        public LogController(ProductionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve the last 100 Eventlog entries
            var eventLogs = await _context.Eventlogs
                .OrderByDescending(e => e.Timestamp) // Assuming you want the most recent logs
                .Take(100)
                .ToListAsync();

            return View(eventLogs);
        }
    }
}
