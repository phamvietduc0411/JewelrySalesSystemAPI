using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<PaginatedList<Customer>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , int page
            , int pageSize);

        Task<Customer?> GetCustomerByNameAsync(string customerName);

        Task<Customer?> GetCustomerByPhoneAsync(string phoneNumber);
    }
}
