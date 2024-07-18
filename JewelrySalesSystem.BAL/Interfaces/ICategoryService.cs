using JewelrySalesSystem.BAL.Models.Categories;
using JewelrySalesSystem.BAL.Models.Products;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface ICategoryService
    {
        Task<List<GetRawCategoryResponse>> GetAllAsync();

        Task<GetCategoryResponse?> GetAllProductsByCategoryIdAsync(int id);

        Task UpdateAsync(UpdateCategoryRequest updateCategoryRequest);

        Task<GetRawCategoryResponse?> GetByIdAsync(int id);

        Task<CreateCategoryRequest> AddNewCategory(CreateCategoryRequest createCategoryRequest);
    }
}
