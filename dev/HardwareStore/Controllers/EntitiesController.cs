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
using HardwareStore.Logic;

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
        public async Task<IActionResult> Details(int? pageNumber, int? id, string searchName,
            int minPrice = 0, int maxPrice = 10000)
        {
            if (id == null || _context.Entity == null)
            {
                return NotFound();
            }

            ViewData["SearchName"] = searchName;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;
            var entity = await _context.Entity.FindAsync(id);
            ViewData["EntityName"] = entity.Name;
            ViewData["Id"] = id;
            ViewData["Entities"] = await _context.Entity.Include(x => x.Categories).ToListAsync();

            var things = from t in _context.Thing.Include(x=>x.Category) 
                         where t.Category.EntityId == id where t.Existence == true where t.Category.Name != "Void" select t;
            if (!String.IsNullOrWhiteSpace(searchName))
            {
                things = things.Where(t => t.Name.Contains(searchName));
            }

            if (minPrice != 0 || maxPrice != 10000)
            {
                things = things.Where(t => t.Price >= minPrice && t.Price <= maxPrice + maxPrice / 10).OrderBy(t => t.Price);
            }

            int pageSize = 12;
            var model = await PaginatedList<Thing>.CreateAsync(things
                .Include(t => t.Images).AsNoTracking(), pageNumber ?? 1, pageSize);

            if (model == null)
            {
                if (pageNumber != 1 && pageNumber != null)
                {
                    return NotFound();
                }
                return View(new PaginatedList<Thing>());
            }
            return View(model);
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

            var entityModel = new EntityCreateViewModel { Id = entity.Id, Name = entity.Name };
            return View(entityModel);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image")] EntityCreateViewModel entityCreateModel)
        {
            if (id != entityCreateModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = _context.Entity.Find(entityCreateModel.Id);
                    entity.Name = entityCreateModel.Name;

                    if (entityCreateModel.Image != null)
                    {
                        if(entity.ImagePath != null)
                        {
                            var imagePath = entity.ImagePath;
                            if (System.IO.File.Exists("wwwroot/images/entities/" + imagePath))
                            {
                                System.IO.File.Delete("wwwroot/images/entities/" + imagePath);
                            }
                        }

                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/entities");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + entityCreateModel.Image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using var fstr = new FileStream(filePath, FileMode.Create);
                        entityCreateModel.Image.CopyTo(fstr);

                        entity.ImagePath = uniqueFileName;
                    }

                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entityCreateModel.Id))
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
            return View(entityCreateModel);
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
