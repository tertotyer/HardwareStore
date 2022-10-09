using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Data;
using HardwareStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using HardwareStore.Logic;

namespace HardwareStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Category.Include(c => c.Entity);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Categories/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? pageNumber, int? id, string searchName,
            int minPrice = 0, int maxPrice = 10000)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            ViewData["SearchName"] = searchName;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            var category = await _context.Category.FindAsync(id);
            ViewData["CategoryName"] = category.Name;
            ViewData["Id"] = id;
            ViewData["Entities"] = await _context.Entity.Include(x => x.Categories).ToListAsync();


            var things = from t in _context.Thing where t.CategoryId == id select t;
            if (!String.IsNullOrWhiteSpace(searchName))
            {
                things = things.Where(t => t.Name.Contains(searchName));
            }

            if (minPrice != 0 || maxPrice != 100000)
            {
                things = things.Where(t => t.Price >= minPrice && t.Price <= maxPrice + maxPrice / 10).OrderBy(t => t.Price);
            }

            int pageSize = 9;
            return View(await PaginatedList<Thing>.CreateAsync(things
                .Include(t => t.Images).AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(things.Include(x => x.Images).ToList());
        }

        // GET: Categories/Create
        public IActionResult Create(int sendEntityId)
        {
            ViewData["SendEntityId"] = sendEntityId;
            ViewData["EntityId"] = new SelectList(_context.Entity, "Id", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,EntityId")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntityId"] = new SelectList(_context.Entity, "Id", "Name", category.EntityId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["EntityId"] = new SelectList(_context.Entity, "Id", "Name", category.EntityId);
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EntityId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["EntityId"] = new SelectList(_context.Entity, "Id", "Name", category.EntityId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.Entity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Category == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return _context.Category.Any(e => e.Id == id);
        }
    }
}
