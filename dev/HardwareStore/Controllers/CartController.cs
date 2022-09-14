using HardwareStore.Data;
using HardwareStore.Logic;
using HardwareStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var things = HttpContext.Session.GetObject<List<Thing>>("cart") ?? new List<Thing>();
            return View(things);
        }

        public async Task<IActionResult> AddToCart(int? id)
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
            HttpContext.Session.AddObject("cart", thing);
            return RedirectToAction("Index");
        }
    }
}
