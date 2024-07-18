using JewelrySalesSystem.BAL.Models.Customers;
using JewelrySalesSystem.DAL.Common;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface ICustomerService
    {
        Task<PaginatedList<GetCustomerResponse>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , int page
            , int pageSize);

        Task AddAsync(CreateCustomerRequest createCustomerRequest);

        Task UpdateAsync(UpdateCustomerRequest updateCustomerRequest);

        Task<GetCustomerResponse?> GetCustomerByNameAsync(string customerName);

        Task<GetCustomerResponse?> GetCustomerByPhoneAsync(string phoneNumber);
    }
}
