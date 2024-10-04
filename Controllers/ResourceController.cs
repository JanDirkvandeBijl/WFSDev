using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var groupedResources = await localizedResources
                       .GroupBy(lr => lr.Key)
                       .ToListAsync();

            return View(groupedResources);
        }

        public async Task<IActionResult> Details(string? key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

            var localizedResources = await _context.LocalizedResources
                .Include(lr => lr.Culture)
                .Where(m => m.Key == key)
                .ToListAsync();

            var resourceModel = new LocalizedResourceDetails
            {
                Key = key,
                Translations = localizedResources
                                    .Select(e => new Translation
                                    {
                                        Id = e.Id,
                                        Value = e.Translation,
                                        CultureId = e.CultureId

                                    }).ToList()
            };


            ViewData["Cultures"] = await _context.Cultures.ToListAsync();
            return View(resourceModel);
        }

       

        private bool LocalizedResourceExists(int id)
        {
            return _context.LocalizedResources.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Cultures"] = await _context.Cultures.ToListAsync();
            return View("Details", new LocalizedResourceDetails());
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocalizedResourceDetails localizedResource)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(localizedResource.Key))
            {
                foreach (var translation in localizedResource.Translations ?? [])
                {
                    // Check if the CultureId and Key pair already exists
                    bool exists = await _context.LocalizedResources
                        .AnyAsync(lr => lr.CultureId == translation.CultureId && lr.Key == localizedResource.Key);

                    if (exists)
                    {
                        ModelState.AddModelError(string.Empty, $"This Culture and Key pair already exists for culture ID {translation.CultureId}.");
                    }
                    else
                    {
                        _context.Add(new LocalizedResource
                        {
                            Key = localizedResource.Key,
                            Translation = translation.Value ?? "",
                            CultureId = translation.CultureId
                        });
                    }
                }

                if (ModelState.IsValid)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Cultures"] = await _context.Cultures.ToListAsync();
            return View("Details", localizedResource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("Key,Translations")] LocalizedResourceDetails localizedResource)
        {
            if (ModelState.IsValid)
            {
                foreach (var translation in localizedResource.Translations ?? [])
                {
                    // Check if the CultureId and Key pair already exists
                    bool exists = await _context.LocalizedResources
                        .AnyAsync(lr => lr.CultureId == translation.CultureId && lr.Key == localizedResource.Key && lr.Id != translation.Id);

                    if (exists)
                    {
                        ModelState.AddModelError(string.Empty, $"This Culture and Key pair already exists for culture ID {translation.CultureId}.");
                    }
                    else
                    {
                        try
                        {
                            var existingResource = await _context.LocalizedResources.FindAsync(translation.Id);
                            if (existingResource != null)
                            {
                                existingResource.Translation = translation.Value ?? "";
                                existingResource.CultureId = translation.CultureId;

                                _context.Update(existingResource);
                            }
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!LocalizedResourceExists(translation.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Cultures"] = await _context.Cultures.ToListAsync();
            return View(localizedResource);
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] string key)
        {
            List<LocalizedResource> resources = await _context.LocalizedResources.Where(lr => lr.Key == key).ToListAsync();
            if (resources == null)
            {
                return Json(new { success = false, message = "Resource not found" });
            }

            _context.LocalizedResources.RemoveRange(resources);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
