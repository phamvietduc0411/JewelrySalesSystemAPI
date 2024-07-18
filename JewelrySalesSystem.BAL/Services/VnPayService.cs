using JewelrySalesSystem.BAL.Helpers;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.VnPays;
using JewelrySalesSystem.DAL.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace JewelrySalesSystem.BAL.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly VnPayConfig _vnPayConfig;

        public VnPayService(
            IHttpContextAccessor httpContextAccessor
            , IOptions<VnPayConfig> vnPayConfigOptions
            , IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnPayConfig = vnPayConfigOptions.Value;
            _unitOfWork = unitOfWork;
        }

        public string CreateUrl(CreatePaymentRequest request)
        {
            var ipAddress = _httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();

            var paymentUrl = string.Empty;

            var vnPayRequest = new VnPayRequest(
                _vnPayConfig.Version,
                _vnPayConfig.TmnCode, 
                DateTime.Now,
                ipAddress ?? string.Empty, 
                request.RequiredAmount ?? 0, 
                "other", 
                request.PaymentContent ?? string.Empty, 
                _vnPayConfig.ReturnUrl, 
                "123456789");

            paymentUrl = vnPayRequest.GetLink(_vnPayConfig.PaymentUrl, _vnPayConfig.HashSecret);

            return paymentUrl;
        }

        public async Task<string> CreateUrl(int id)
        {
            var ipAddress = _httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();

            var paymentUrl = string.Empty;

            var invoice = await _unitOfWork.Invoices.GetEntityByIdAsync(id);

            if (invoice != null)
            {
                var vnPayRequest = new VnPayRequest(
                _vnPayConfig.Version,
                _vnPayConfig.TmnCode,
                DateTime.Now,
                ipAddress ?? string.Empty,
                (decimal)invoice.TotalWithDiscount,
                "other",
                $"Check-out order {invoice.InvoiceId}" ?? string.Empty,
                _vnPayConfig.ReturnUrl,
                invoice.InvoiceId.ToString());

                paymentUrl = vnPayRequest.GetLink(_vnPayConfig.PaymentUrl, _vnPayConfig.HashSecret);
            }

            return paymentUrl;
        }

        public async Task<VnPayResponse> ExecutePayment(IQueryCollection collections)
        {
            var vnPayLibrary = new VnPayLibrary();
            foreach(var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPayLibrary.AddResponseData(key, value.ToString());
                }
            }

            var vnp_OrderId = Convert.ToInt64(vnPayLibrary.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnPayLibrary.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnPayLibrary.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnPayLibrary.GetResponseData("vnp_OrderInfo");

            if (vnp_ResponseCode.Equals("00"))
            {
                var invoice = await _unitOfWork.Invoices.GetEntityByIdAsync((int)vnp_OrderId);

                if (invoice != null)
                {
                    invoice.InvoiceStatus = "Delivered";

                    _unitOfWork.Invoices.UpdateEntity(invoice);

                    await _unitOfWork.CompleteAsync();
                }
            }

            bool checkSignature = vnPayLibrary.ValidateSignature(vnp_SecureHash, _vnPayConfig.HashSecret);
            if (!checkSignature)
            {
                return new VnPayResponse
                {
                    Success = false
                };
            }

            return new VnPayResponse
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_OrderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };
        }
    }
}
