using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<PaginatedList<Product>> JewelryPaginationAsync
            (int productTypeId
            , int? counterId
            , int? categoryId
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<PaginatedList<Product>> PaginationAsync
            (int productTypeId
            , int? counterId
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<Product?> GetByIdWithIncludeAsync(int id);

        Task<Product?> GetByNameAsync(string name);

        Task<List<Product>> GetJewelryAndMaterialProducts();

        void UpdateAllProducts(List<Product> products);

        Task UpdateProduct(Product product);

        Task DeleteProduct(int id);

        Task<float> GetWeightAsync(int productId, int materialId);

        //changes here
        Task<Product> CheckDuplicate(string productName);

        Task<bool> CategoryExit(int categoryId);

        Task<bool> ProductTypeExit(int productTypeId);

        Task<bool> GenderExit(int genderId);
    }
}
