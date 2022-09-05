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
    public class EntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Entities
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Entity.Include(e => e.Title);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Entities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Entity == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity
                .Include(e => e.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Entities/Create
        public IActionResult Create()
        {
            ViewData["TitleId"] = new SelectList(_context.Title, "ID", "ID");
            return View();
        }

        // POST: Entities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TitleId")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TitleId"] = new SelectList(_context.Title, "ID", "ID", entity.TitleId);
            return View(entity);
        }

        // GET: Entities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Entity == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            ViewData["TitleId"] = new SelectList(_context.Title, "ID", "ID", entity.TitleId);
            return View(entity);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TitleId")] Entity entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entity.Id))
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
            ViewData["TitleId"] = new SelectList(_context.Title, "ID", "ID", entity.TitleId);
            return View(entity);
        }

        // GET: Entities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Entity == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity
                .Include(e => e.Title)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Entities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Entity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Entity'  is null.");
            }
            var entity = await _context.Entity.FindAsync(id);
            if (entity != null)
            {
                _context.Entity.Remove(entity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntityExists(int id)
        {
          return _context.Entity.Any(e => e.Id == id);
        }
    }
}
