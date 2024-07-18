using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<PaginatedList<Invoice>> PaginationAsync
            (string? invoiceStatus
            , string? invoiceType
            , string? inOrOut
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<Invoice?> GetByIdWithIncludeAsync(int id);

        Task DeleteById(int id);

        Task UpdateInvoice(Invoice invoice);

        //Task<Invoice> AddPurchaseInvoice(Invoice invoice);

        Task ChangeInvoiceStatus(int id);

        Task ChangePendingToDraft(int id);

        Task CancelInvoice(int id);

        Task<List<Invoice>> GetInvoicesForMonthAsync(int month, int year);

        Task<bool> CheckValidYear(int year);

        Task<List<Invoice>> GetMonthlyRevenue( int month , int year);

        Task<List<Invoice>> GetDailyRevenue(int day,int month, int year);

        Task<List<int>> GetMonthlyProductSalesAsync(int month, int year);

    }
}
