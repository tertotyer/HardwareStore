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
            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("cart") ?? new List<CartItem>();
            return View(cartItems);
        }

        public IActionResult AddToCart(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var cartItem = new CartItem {
                ThingId = (int)id,
                Thing = _context.Thing.Find(id)
            };
            HttpContext.Session.AddObject("cart", cartItem);

            return RedirectToAction("Index");
        } 

        public IActionResult RemoveFromCart(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var cartItem = HttpContext.Session.GetObject<List<CartItem>>("cart").Where(x=> x.ThingId == id).First();
            HttpContext.Session.RemoveObject("cart", ref cartItem);
            
            return RedirectToAction("Index");
        }

        public IActionResult ChangeItemQuantity(int? id, int quantity)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("cart");
            cartItems.Where(x => x.ThingId == id).First().Quantity = quantity;

            HttpContext.Session.SetObject("cart", cartItems);

            return RedirectToAction("Index");
        }
    }
}
