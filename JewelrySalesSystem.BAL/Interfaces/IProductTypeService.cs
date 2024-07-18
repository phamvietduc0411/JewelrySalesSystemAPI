using JewelrySalesSystem.BAL.Models.ProductTypes;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IProductTypeService
    {
        Task<ProductTypeIdCollectionResponse?> GetAllProductsByProductTypeIdAsync(int productTypeId); 

        Task<GetProductTypeResponse?> GetByIdAsync(int id);

        Task<List<GetProductTypeResponse>> GetAllAsync();

        Task<CreateProductTypeRequest> AddAsync(CreateProductTypeRequest productType);

        Task<UpdateTypeRequest> UpdateAsync (UpdateTypeRequest productType);
    }
}
