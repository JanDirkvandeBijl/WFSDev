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
                searchString = searchString.ToLower();
                localizedResources = localizedResources.Where(s => s.Key.ToLower().Contains(searchString) || s.Translation.ToLower().Contains(searchString));
            }
            var groupedResources = await localizedResources
                       .GroupBy(lr => lr.Key)
                       .ToListAsync();

            return View(groupedResources);
        }

       

        private bool LocalizedResourceExists(int id)
        {
            return _context.LocalizedResources.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string key = "")
        {
            ViewData["Cultures"] = await _context.Cultures.ToListAsync();

            if (string.IsNullOrEmpty(key))
            {
                // Nieuwe
                return View(new LocalizedResourceDetails());
            }
            else
            {
                var localizedResource = new LocalizedResourceDetails
                {
                    Key = key,
                    Translations = await _context.LocalizedResources
                        .Where(lr => lr.Key == key)
                        .Select(lr => new Translation
                        {
                            Id = lr.Id,
                            CultureId = lr.CultureId,
                            Value = lr.Translation
                        }).ToListAsync()
                };

                return View(localizedResource);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(LocalizedResourceDetails localizedResource)
        {
            localizedResource.Key = localizedResource.Key?.Trim();
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(localizedResource.Key))
            {
                if (localizedResource.SubmitType == SubmitType.Create)
                {
                    // Check if the resource already exists
                    var existingResource = await _context.LocalizedResources
                        .FirstOrDefaultAsync(lr => lr.Key == localizedResource.Key);

                    if (existingResource != null)
                    {
                        ModelState.AddModelError(string.Empty, "A resource with this key already exists.");
                        ViewData["Cultures"] = await _context.Cultures.ToListAsync();
                        return View(localizedResource);
                    }
                }

                foreach (var translation in localizedResource.Translations ?? new List<Translation>())
                {
                    if (!string.IsNullOrWhiteSpace(translation.Value))
                    {
                        var existingTranslation = await _context.LocalizedResources
                            .FirstOrDefaultAsync(lr => lr.CultureId == translation.CultureId && lr.Id == translation.Id);

                        if (existingTranslation != null)
                        {
                            // Update existing resource
                            existingTranslation.Key = localizedResource.Key;
                            existingTranslation.Translation = translation.Value;
                            _context.Update(existingTranslation);
                        }
                        else
                        {
                            // Create new resource
                            _context.Add(new LocalizedResource
                            {
                                Key = localizedResource.Key,
                                Translation = translation.Value,
                                CultureId = translation.CultureId
                            });
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
