using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Repositories;
using PD411_Shop.Services;
using PD411_Shop.ViewModels;

namespace PD411_Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCount(int id, IEnumerable<CartItemVM> items)
        {
            return items.FirstOrDefault(i => i.ProductId == id)!.Count;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = CartService.GetItems(HttpContext.Session).ToList();
            var ids = cartItems.Select(i => i.ProductId);
            List<CartProductVM> items = [];

            if (cartItems.Count() > 0)
            {
                var products = await _context.Products
                    .Where(p => ids.Contains(p.Id))
                    .ToListAsync();

                for (int i = 0; i < products.Count(); i++)
                {
                    var item = new CartProductVM
                    {
                        Product = products[i],
                        Count = cartItems[i].Count
                    };
                    items.Add(item);
                }
            }

            return View(items);
        }

        public IActionResult Increment(int productId)
        {
            CartService.Increment(HttpContext.Session, productId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Add(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                CartService.AddToCart(HttpContext.Session, id);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
