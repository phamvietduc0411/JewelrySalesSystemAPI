using JewelrySalesSystem.BAL.Models.MaterialPriceList;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IMaterialPriceListService
    {
        Task<List<CreateMaterialPriceList>> AddAsync (List<CreateMaterialPriceList> prices);
    }
}
