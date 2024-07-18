using JewelrySalesSystem.BAL.Models.Materials;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IMaterialService
    {
        Task<PaginatedList<GetMaterialResponse>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<List<GetMaterialResponse>> GetAllGoldMaterials();

        Task<CreateMaterialRequest> AddAsync(CreateMaterialRequest createMaterialRequest);

        Task<GetMaterialResponse?> GetByIdAsync(int id);

        Task DeleteAsync(int id);

        Task UpdateAsync(UpdateMaterialRequest updateMaterialRequest);

        Task<GetMaterialResponse?> GetByIdWithIncludeAsync(int id);
    }
}
