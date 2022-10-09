using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Data;
using HardwareStore.Models;
using HardwareStore.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HardwareStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class ThingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? pageNumber, string searchName,
            int minPrice = 0, int maxPrice = 100000)
        {
            ViewData["SearchName"] = searchName;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;

            var things = from t in _context.Thing select t;

            if (!String.IsNullOrWhiteSpace(searchName))
            {
                things = things.Where(t => t.Name.Contains(searchName));
            }

            if(minPrice != 0 || maxPrice != 100000)
            {
                things = things.Where(t => t.Price >= minPrice && t.Price <= maxPrice+maxPrice/10).OrderBy(t => t.Price);
            }

            int pageSize = 4;
            return View(await PaginatedList<Thing>.CreateAsync(things.Include(t => t.Category)
                .Include(t => t.Images).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Things/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Thing == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing
                .Include(x => x.Images).Include(x => x.Characteristics)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thing == null)
            {
                return NotFound();
            }

            return View(thing);
        }

        // GET: Things/Create
        public IActionResult Create(int sendCategoryId)
        {
            ViewData["SendCategoryId"] = sendCategoryId;
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Things/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,CategoryId")] Thing thing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thing);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Categories", new { id = thing.CategoryId } );
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", thing.CategoryId);
            return View(thing);
        }

        // GET: Things/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Thing == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing.FindAsync(id);
            if (thing == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", thing.CategoryId);
            return View(thing);
        }

        // POST: Things/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CategoryId")] Thing thing)
        {
            if (id != thing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThingExists(thing.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", thing.CategoryId);
            return View(thing);
        }
        
        // GET: Things/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Thing == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thing == null)
            {
                return NotFound();
            }

            return View(thing);
        }

        // POST: Things/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Thing == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Thing'  is null.");
            }

            var thing = await _context.Thing.Include(x=> x.Images).Where(x=> x.Id == id).FirstAsync();
            var images = thing.Images;

            if (thing != null)
            {
                _context.Thing.Remove(thing);
            }
            await _context.SaveChangesAsync();

            foreach (var image in images)
            {
                var imagePath = image.ImagePath;
                if (System.IO.File.Exists("wwwroot/images/" + imagePath))
                {
                    System.IO.File.Delete("wwwroot/images/" + imagePath);
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ThingExists(int id)
        {
            return _context.Thing.Any(e => e.Id == id);
        }
    }
}
