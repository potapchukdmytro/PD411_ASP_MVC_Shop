using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.ViewModels;

namespace PD411_Shop.Repositories
{
    public class ProductRepository
        : GenericRepository<ProductModel>
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductModel>> GetAllAsync(PaginationVM pagination, int? category = null)
        {
            IQueryable<ProductModel> products = 
                _context.Products
                .AsNoTracking()
                .Include(p => p.Category);

            if (category != null && _context.Categories.Any(c => c.Id == category))
            {
                products = products.Where(p => p.CategoryId == category);
            }

            // Pagination
            pagination.PageSize = pagination.PageSize < 1 ? 20 : pagination.PageSize;
            pagination.PageCount = (int)Math.Ceiling((double)products.Count() / pagination.PageSize);
            pagination.Page = pagination.Page < 1 || pagination.Page > pagination.PageCount ? 1 : pagination.Page;

            products = products
                .OrderBy(p => p.Id)
                .Skip(pagination.PageSize * (pagination.Page - 1))
                .Take(pagination.PageSize);

            return await products.ToListAsync();
        }
    }
}
