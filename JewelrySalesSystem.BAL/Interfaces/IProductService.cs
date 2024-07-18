using JewelrySalesSystem.BAL.Models.Products;
using JewelrySalesSystem.DAL.Common;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedList<GetProductResponse>> ProductPaginationAsync
            (int productTypeId
            , int? counterId
            , int? categoryId
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<PaginatedList<GetGemProductResponse>> GemPaginationAsync
            (int productTypeId
            , int? counterId
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<PaginatedList<GetMaterialProductResponse>> MaterialPaginationAsync
            (int productTypeId
            , int? counterId
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<CreateProductRequest> AddAsync(CreateProductRequest createProductRequest);

        Task UpdateProductPriceAsync();

        Task UpdateAsync(UpdateProductRequest updateProductRequest);

        Task<GetProductResponse?> GetByIdAsync(int id);

        Task<GetProductResponse?> GetByIdWithIncludeAsync(int id);

        Task DeleteAsync(int id);
    }
}
