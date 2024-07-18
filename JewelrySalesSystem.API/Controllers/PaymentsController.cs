using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentsController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        //[HttpPost]
        //public IActionResult Create([FromBody] CreatePaymentRequest request)
        //{
        //    var url = _vnPayService.CreateUrl(request);

        //    return Ok(url);
        //}

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = await _vnPayService.ExecutePayment(Request.Query);

            if (response != null)
            {
                if (response.VnPayResponseCode == "00")
                {
                    return Redirect("http://localhost:5173/payment/success");
                }
                else
                {
                    return BadRequest(new
                    {
                        ErrorMessage = response.VnPayResponseCode
                    });
                }
            }
            return NoContent();
        }
    }
}
