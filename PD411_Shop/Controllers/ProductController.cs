using Microsoft.AspNetCore.Mvc;

namespace PD411_Shop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
