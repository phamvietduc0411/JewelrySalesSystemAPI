using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IWarrantyRepository : IGenericRepository<Warranty>
    {
        Task<PaginatedList<Warranty>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , int page
            , int pageSize);

        /*Initialized interface update warranty method*/
        void UpdateWarranty (Warranty warranty);

        //Change here
        Task<Warranty> CreateWarranty(Warranty warranty);
    }

    
}
