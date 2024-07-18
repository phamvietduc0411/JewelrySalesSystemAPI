using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IProductTypeRepository : IGenericRepository<ProductType>
    {
        Task<ProductType?> GetAllProductsByProductTypeIdAsync(int productTypeId);

        Task<List<ProductType>> GetAllAsync();
        Task<ProductType> CheckDuplicate(string productType);
    }
}
