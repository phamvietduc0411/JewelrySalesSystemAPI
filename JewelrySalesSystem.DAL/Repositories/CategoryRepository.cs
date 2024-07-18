using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(
            JewelryDbContext context,
            ILogger logger) : base(context, logger)
        {
        }

        public async Task<Category> CheckDuplicate(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CategoryName == name);
        }

        public async Task<List<Category>> GetAllAsync() => await _dbSet
                                                                 .OrderByDescending(x => x.CategoryId)
                                                                 .Where(c => c.IsActive)
                                                                 .ToListAsync();

        public async Task<Category?> GetAllProductsByCategoryIdAsync(int id)
            => await _dbSet.Include(c => c.Products)
                                .ThenInclude(p => p.Gender)                            
                            .Include(c => c.Products)
                                .ThenInclude(p => p.ProductType)
                           .FirstOrDefaultAsync(c => c.IsActive && c.CategoryId == id);
    }
}
