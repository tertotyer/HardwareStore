using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Data;
using HardwareStore.Models;
using HardwareStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HardwareStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ImagesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Image.Include(i => i.Thing);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Image == null)
            {
                return NotFound();
            }

            var image = await _context.Image
                .Include(i => i.Thing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create(int sendThingId)
        {
            ViewData["SendThingId"] = sendThingId;
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "Name");
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageCreateViewModel imageCreateModel)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageCreateModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fstr = new FileStream(filePath, FileMode.Create);
                imageCreateModel.Image.CopyTo(fstr);

                Image newImage = new Image
                {
                    ImagePath = uniqueFileName,
                    ThingId = imageCreateModel.ThingId
                };


                _context.Add(newImage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Things");
            }
            ViewData["SendThingId"] = imageCreateModel.ThingId;
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "Name", imageCreateModel.ThingId);
            return View();
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Image == null)
            {
                return NotFound();
            }

            var image = await _context.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "Name", image.ThingId);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImagePath,ThingId")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
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
            ViewData["ThingId"] = new SelectList(_context.Thing, "Id", "Name", image.ThingId);
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Image == null)
            {
                return NotFound();
            }

            var image = await _context.Image
                .Include(i => i.Thing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Image == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Image'  is null.");
            }
            var image = await _context.Image.FindAsync(id);
            if (image != null)
            {
                _context.Image.Remove(image);
            }

            await _context.SaveChangesAsync();

            var imagePath = image.ImagePath;
            if (System.IO.File.Exists("wwwroot/images/" + imagePath))
            {
                System.IO.File.Delete("wwwroot/images/" + imagePath);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.Id == id);
        }
    }
}
