using JewelrySalesSystem.BAL.Models.VnPays;
using Microsoft.AspNetCore.Http;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IVnPayService
    {
        public string CreateUrl(CreatePaymentRequest request);
        public Task<string> CreateUrl(int id);
        Task<VnPayResponse> ExecutePayment(IQueryCollection collections);
    }
}
