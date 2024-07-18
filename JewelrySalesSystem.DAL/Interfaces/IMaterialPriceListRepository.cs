using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IMaterialPriceListRepository : IGenericRepository<MaterialPriceList>
    {
        Task<MaterialPriceList> CreateMaterialPrice(MaterialPriceList materialPrice);

        Task<MaterialPriceList> GetNewestMaterialPriceByMaterialIdAsync(int materialId);
    }
}
