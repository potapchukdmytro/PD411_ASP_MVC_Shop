namespace PD411_Shop.Models
{
    public class CategoryModel : BaseModel
    {
        public required string Name { get; set; }

        public List<ProductModel> Products { get; set; } = [];
    }
}
