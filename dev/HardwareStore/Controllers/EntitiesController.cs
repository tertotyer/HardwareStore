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
using HardwareStore.ViewModels;

namespace HardwareStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class EntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EntitiesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Entities
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Entity.ToListAsync());
        }

        // GET: Entities/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Entity == null)
            {
                return NotFound();
            }

            var entity = await _context.Entity.Include(x => x.Categories)
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
            return View();
        }

        // POST: Entities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntityCreateViewModel entityCreateModel)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/entities");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + entityCreateModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fstr = new FileStream(filePath, FileMode.Create);
                entityCreateModel.Image.CopyTo(fstr);


                Entity entity = new Entity
                {
                    Name = entityCreateModel.Name,
                    ImagePath = uniqueFileName
                };

                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entityCreateModel);
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
            return View(entity);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Entity entity)
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

            var imagePath = entity.ImagePath;
            if (System.IO.File.Exists("wwwroot/images/entities/" + imagePath))
            {
                System.IO.File.Delete("wwwroot/images/entities/" + imagePath);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EntityExists(int id)
        {
            return _context.Entity.Any(e => e.Id == id);
        }
    }
}
