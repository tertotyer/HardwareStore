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

namespace HardwareStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class CharacteristicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacteristicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Characteristics
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Characteristic.Include(c => c.Thing);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Characteristics/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Characteristic == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristic
                .Include(c => c.Thing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characteristic == null)
            {
                return NotFound();
            }

            return View(characteristic);
        }

        // GET: Characteristics/Create
        public IActionResult Create(string sendThingId)
        {
            ViewData["SendThingId"] = sendThingId;
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "Name");
            return View();
        }

        // POST: Characteristics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ThingId,Name,Data")] Characteristic characteristic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characteristic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", new { sendThingId = characteristic.ThingId});
            }
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "Name", characteristic.ThingId);
            return View(characteristic);
        }

        // GET: Characteristics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Characteristic == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristic.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound();
            }
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "Name", characteristic.ThingId);
            return View(characteristic);
        }

        // POST: Characteristics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ThingId,Name,Data")] Characteristic characteristic)
        {
            if (id != characteristic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characteristic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacteristicExists(characteristic.Id))
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
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "ModelName", characteristic.ThingId);
            return View(characteristic);
        }

        // GET: Characteristics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Characteristic == null)
            {
                return NotFound();
            }

            var characteristic = await _context.Characteristic
                .Include(c => c.Thing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characteristic == null)
            {
                return NotFound();
            }

            return View(characteristic);
        }

        // POST: Characteristics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Characteristic == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Characteristic'  is null.");
            }
            var characteristic = await _context.Characteristic.FindAsync(id);
            if (characteristic != null)
            {
                _context.Characteristic.Remove(characteristic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacteristicExists(int id)
        {
          return _context.Characteristic.Any(e => e.Id == id);
        }
    }
}
