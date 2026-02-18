using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.ViewModels;
using System.Collections;
using System.Diagnostics;

namespace PD411_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? category)
        {
            List<CategoryModel> categories = _context.Categories.ToList();
            IQueryable<ProductModel> products = _context.Products;

            if (category != null && categories.Any(c => c.Id == category))
            {
                products = products.Where(p => p.CategoryId == category);
            }

            var homeVm = new HomeVM
            {
                Products = products,
                Categories = categories
            };

            return View(homeVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
