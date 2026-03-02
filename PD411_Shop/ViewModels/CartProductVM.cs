using PD411_Shop.Models;

namespace PD411_Shop.ViewModels
{
    public class CartProductVM
    {
        public ProductModel? Product { get; set; }
        public int Count { get; set; } = 1;
    }
}
