using JewelrySalesSystem.BAL.Models.Warranties;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IWarrantyService
    {
        Task<PaginatedList<GetWarrantyResponse>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , int page
            , int pageSize);

        Task UpdateAsync(UpdateWarrantyRequest updateWarrantyRequest);

        Task<GetWarrantyResponse?>GetWarrantyById(int id);

        Task<CreateWarrantyRequest> AddNewWarranty(CreateWarrantyRequest createWarrantyRequest);
    }
}
