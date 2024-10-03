using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WFSDev.Data;
using WFSDev.Models;

namespace WFSDev.Controllers
{
    [Authorize]
    public class ResourceController : Controller
    {
        private readonly ProductionDbContext _context;

        public ResourceController(ProductionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var localizedResources = from lr in _context.LocalizedResources
                                     .Include(lr => lr.Culture)
                                     select lr;

            if (!string.IsNullOrEmpty(searchString))
            {
                localizedResources = localizedResources.Where(s => s.Key.Contains(searchString) || s.Translation.Contains(searchString));
            }

            return View(await localizedResources.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localizedResource = await _context.LocalizedResources
                .Include(lr => lr.Culture)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (localizedResource == null)
            {
                return NotFound();
            }

            ViewData["Cultures"] = new SelectList(await _context.Cultures.ToListAsync(), "Id", "Name");
            return View(localizedResource);
        }

       

        private bool LocalizedResourceExists(int id)
        {
            return _context.LocalizedResources.Any(e => e.Id == id);
        }

        public IActionResult Create()
        {
            ViewData["Cultures"] = new SelectList(_context.Cultures, "Id", "Name");
            return View("Details", new LocalizedResource());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CultureId,Key,Translation")] LocalizedResource localizedResource)
        {
            if (ModelState.IsValid)
            {
                // Check if the CultureId and Key pair already exists
                bool exists = await _context.LocalizedResources
                    .AnyAsync(lr => lr.CultureId == localizedResource.CultureId && lr.Key == localizedResource.Key);

                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "This Culture and Key pair already exists.");
                }
                else
                {
                    _context.Add(localizedResource);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Cultures"] = new SelectList(_context.Cultures, "Id", "Name", localizedResource.CultureId);
            return View("Details", localizedResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,CultureId,Key,Translation")] LocalizedResource localizedResource)
        {
            if (id != localizedResource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if the CultureId and Key pair already exists
                bool exists = await _context.LocalizedResources
                    .AnyAsync(lr => lr.CultureId == localizedResource.CultureId && lr.Key == localizedResource.Key && lr.Id != localizedResource.Id);

                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "This Culture and Key pair already exists.");
                }
                else
                {
                    try
                    {
                        _context.Update(localizedResource);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LocalizedResourceExists(localizedResource.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Cultures"] = new SelectList(_context.Cultures, "Id", "Name", localizedResource.CultureId);
            return View(localizedResource);
        }

        [HttpPost]
        public IActionResult Delete([FromBody] int id)
        {
            var resource = _context.LocalizedResources.Find(id);
            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found" });
            }

            _context.LocalizedResources.Remove(resource);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
