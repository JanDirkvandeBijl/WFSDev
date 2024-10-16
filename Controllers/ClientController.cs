using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFSDev.Data;
using WFSDev.Models;

namespace WFSDev.Controllers
{
    public class ClientController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

     
        // Add actions for Client and ClientConnectionString
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients.Include(c => c.ConnectionStrings).ToListAsync();
            return View(clients);
        }

        public async Task<IActionResult> Details(int id = 0)
        {
            if (id == 0)
            {
                // New client
                return View(new Client());
            }
            else
            {
                var client = await _context.Clients
                    .Include(c => c.ConnectionStrings)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (client == null)
                {
                    return NotFound();
                }

                return View(client);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Client client)
        {
            if (ModelState.IsValid)
            {
                if (client.Id == 0)
                {
                    _context.Clients.Add(client);
                }
                else
                {
                    _context.Clients.Update(client);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(client);
        }

        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ConnectionStringDetails(int id = 0, int clientId = 0)
        {
            if (id == 0)
            {
                // New connection string
                return View(new ConnectionString { ClientId = clientId });
            }
            else
            {
                var connectionString = await _context.ConnectionStrings
                    .FirstOrDefaultAsync(cs => cs.Id == id);

                if (connectionString == null)
                {
                    return NotFound();
                }

                return View(connectionString);
            }
        }

        // POST: ConnectionStringDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConnectionStringDetails(ConnectionString connectionString)
        {
            if (ModelState.IsValid)
            {
                if (connectionString.Id == 0)
                {
                    _context.ConnectionStrings.Add(connectionString);
                }
                else
                {
                    _context.ConnectionStrings.Update(connectionString);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("ClientDetails", new { id = connectionString.ClientId });
            }

            return View(connectionString);
        }
    }
}
