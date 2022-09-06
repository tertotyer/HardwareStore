using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Data;
using HardwareStore.Models;

namespace HardwareStore.Controllers
{
    public class ThingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Things
        public async Task<IActionResult> Index()
        {
              return View(await _context.Thing.ToListAsync());
        }

        // GET: Things/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Thing == null)
            {
                return NotFound();
            }

            var thing = await _context.Thing
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thing == null)
            {
                return NotFound();
            }

            return View(thing);
        }

        // GET: Things/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Things/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelName,EntityId")] Thing thing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(thing);
        }

        // POST: Things/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelName,EntityId")] Thing thing)
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
            var thing = await _context.Thing.FindAsync(id);
            if (thing != null)
            {
                _context.Thing.Remove(thing);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThingExists(int id)
        {
          return _context.Thing.Any(e => e.Id == id);
        }
    }
}
