using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IMaterialRepository : IGenericRepository<Material>
    {
        Task<PaginatedList<Material>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<Material?> GetByNameWithIncludeAsync(string name);

        Task<List<Material>> GetAllGoldMaterials();

        Task<Material?> GetByIdWithIncludeAsync(int id);

        //changes here
        Task<Material> CheckDuplicate(string materialName);
    }
}
