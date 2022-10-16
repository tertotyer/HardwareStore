using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HardwareStore.Data;
using HardwareStore.Models;
using System.Net.NetworkInformation;
using HardwareStore.Logic;
using NuGet.Packaging;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using HardwareStore.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HardwareStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(o => o.CartItems).ThenInclude(x => x.Thing)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewBag.CartItems = HttpContext.Session.GetObject<List<CartItemSession>>("cart");

            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateCreated,NameBuyer,Email,PhoneNumber,DeliveryMethod")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();

                var cartItems = HttpContext.Session.GetObject<List<CartItemSession>>("cart");
                if (cartItems == null)
                {
                    return NotFound();
                }
                foreach (var cartItem in cartItems)
                {
                    cartItem.Thing = null;
                }
                order.CartItems.AddRange(cartItems);
                HttpContext.Session.Clear();
                await _context.SaveChangesAsync();

                ViewBag.Order = "Ordered :)";
                return Redirect("../#popup__order-complete");
                //return RedirectToAction("Index", "Home", new { id = 1 });
            }

            ViewData["CartItems"] = HttpContext.Session.GetObject<List<CartItemSession>>("cart");

            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
