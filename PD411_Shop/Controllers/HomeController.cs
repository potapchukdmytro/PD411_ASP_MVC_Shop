using Microsoft.AspNetCore.Mvc;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.ViewModels;
using System.Diagnostics;

namespace PD411_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int? category, [FromQuery] PaginationVM pagination)
        {
            var categories = await _categoryRepository.GetAllAsync();

            var homeVm = new HomeVM
            {
                Products = await _productRepository.GetAllAsync(pagination, category),
                Categories = categories,
                Pagination = pagination,
                CategoryId = category
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
