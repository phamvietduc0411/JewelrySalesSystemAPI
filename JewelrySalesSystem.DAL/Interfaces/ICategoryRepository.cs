using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetAllProductsByCategoryIdAsync(int id);

        Task<Category> CheckDuplicate(string name);
    }
}
