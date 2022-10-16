using HardwareStore.Data;
using HardwareStore.Logic;
using HardwareStore.Models;
using HardwareStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

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
            var cartItems = HttpContext.Session.GetObject<List<CartItemSession>>("cart") ?? new List<CartItemSession>();
            return View(cartItems);
        }

        public async Task<IActionResult> AddToCart(string id, string imagePath)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var cartItems = HttpContext.Session.GetObject<List<CartItemSession>>("cart") ?? new List<CartItemSession>();
            if (cartItems.Any(x => x.ThingId == id))
            {
                cartItems.Where(x => x.ThingId == id).First().Quantity += 1;
                HttpContext.Session.SetObject("cart", cartItems);
            }
            else
            {
                var cartItem = new CartItemSession
                {
                    ThingId = id,
                    Thing = await _context.Thing.FindAsync(id),
                    ImagePath = imagePath
                };
                HttpContext.Session.AddObject("cart", cartItem);
            }


            return RedirectToAction("Create", "Orders");
        }

        public IActionResult RemoveFromCart(string id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var cartItem = HttpContext.Session.GetObject<List<CartItemSession>>("cart").Where(x => x.ThingId == id).First();
            HttpContext.Session.RemoveObject("cart", cartItem);

            return RedirectToAction("Create", "Orders");
        }

        public IActionResult ChangeItemQuantity(string id, int quantity)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var cartItems = HttpContext.Session.GetObject<List<CartItemSession>>("cart");
            cartItems.Where(x => x.ThingId == id).First().Quantity = quantity;

            HttpContext.Session.SetObject("cart", cartItems);

            return RedirectToAction("Create", "Orders");
        }
    }
}
