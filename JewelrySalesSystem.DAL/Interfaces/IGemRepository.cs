using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IGemRepository : IGenericRepository<Gem>
    {
        Task<PaginatedList<Gem>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<Gem?> GetByNameWithIncludeAsync(string name);

        Task<Gem?> GetByIdWithIncludeAsync(int id);

        Task<float> GetGemPriceAsync(Gem gem);

        Task<float> GetShapePriceRateAsync(int shapeId);

        Task DeleteAsync(int id);

        Task UpdateGem(Gem gem);

        Task<bool> CheckId(int id,string option);

        Task<float> GetGemPriceAsync(GemPriceList gemPriceList);

        Task<List<GemPriceList>> GetGemPricesAsync();

        void AddGemPrice(GemPriceList gemPriceList);
    }
}
