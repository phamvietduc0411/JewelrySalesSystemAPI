using JewelrySalesSystem.BAL.Models.Gems;
using JewelrySalesSystem.BAL.Models.Warranties;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IGemService
    {
        Task<PaginatedList<GetGemResponse>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<CreateGemRequest> AddAsync(CreateGemRequest createGemRequest);

        Task UpdateAsync(UpdateGemRequest updateGemRequest);

        Task<GetGemResponse?> GetByIdWithIncludeAsync(int id);

        Task<GetGemResponse?> GetGemById(int id);

        Task<float> GetGemPriceAsync(GemPriceRequest gemPriceRequest);

        Task<List<GetGemPriceResponse>> GetGemPricesAsync();

        Task DeleteAsync(int id);
    }
}
