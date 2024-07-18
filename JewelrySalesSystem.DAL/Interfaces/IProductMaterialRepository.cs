using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IProductMaterialRepository : IGenericRepository<ProductMaterial>
    {
        Task<float> GetProductMaterialWeightAsync(int materialId);

        public Task<ProductMaterial> GetProductMaterialByProductIdAsync(int productId);
    }
}
