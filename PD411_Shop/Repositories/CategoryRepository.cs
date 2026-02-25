using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;

namespace PD411_Shop.Repositories
{
    public class CategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CategoryModel?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task CreateAsync(CategoryModel model)
        {
            await _context.Categories.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryModel model)
        {
            _context.Categories.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var model = await GetByIdAsync(id);
            if (model != null)
            {
                _context.Categories.Remove(model);
                await _context.SaveChangesAsync();
            }
        }
    }
}
