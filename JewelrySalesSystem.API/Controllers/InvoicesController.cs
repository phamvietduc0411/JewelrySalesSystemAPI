using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Invoices;
using JewelrySalesSystem.BAL.Validators.Invoices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/invoices")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IWarrantyService _warrantyService;
        private readonly IProductService _productService;
        private readonly IVnPayService _vnPayService;


        public InvoicesController(
            ILogger<InvoicesController> logger,
            IInvoiceService invoiceService, ICustomerService customerService, IUserService userService, IWarrantyService warrantyService, IProductService productService, IVnPayService vnPayService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
            _customerService = customerService;
            _userService = userService;
            _productService = productService;
            _warrantyService = warrantyService;
            _vnPayService = vnPayService;
        }

        #region Get All Invoices
        /// <summary>
        /// Get all invoices in the system
        /// </summary>
        /// <param name="invoiceStatus">Invoice Status (Draft, Pending, Processing, Delivered, Cancelled)</param>
        /// <param name="invoiceType">Invoice Type (Sale, Purchase)</param>
        /// <param name="inOrOut">Purchase In or Out (In, Out)</param>
        /// <param name="page">Current page the user is on</param>
        /// <param name="pageSize">Number of entities you want to show</param>
        /// <param name="isActive">Invoice active or not</param>
        /// <param name="searchTerm">Search query</param>
        /// <param name="sortColumn">Column you want to sort</param>
        /// <param name="sortOrder">Sort column by ascending or descening</param>
        /// <returns>A list of all users</returns>
        /// <response code="200">Return all invoices in the system</response>
        /// <response code="400">If no invoices are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            string? invoiceStatus,
            string? invoiceType,
            string? inOrOut,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page = 1,
            int pageSize = 5)
        {
            try
            {
                var result = await _invoiceService.PaginationAsync(invoiceStatus, invoiceType, inOrOut, searchTerm, sortColumn, sortOrder, isActive, page, pageSize);

                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return BadRequest();
        }
        #endregion

        #region Add Invoice
        /// <summary>
        /// Add an invoice in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "customerName": "Tran Thi A",
        ///       "invoiceStatus": "Draft",
        ///       "phoneNumber": "0912345789",
        ///       "userId": 1,
        ///       "total": 15000000,
        ///       "perDiscount": 10,
        ///       "invoiceDetails": [
        ///         {
        ///           "productId": 2,
        ///           "quantity": 1
        ///         },
        ///         {
        ///           "productId": 3,
        ///           "quantity": 2
        ///         }
        ///       ]
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Invoice that was created</returns>
        /// <response code="200">Invoice that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateInvoiceRequest createInvoiceRequest)
        {

            try
            {
                //Use Fluent Validation
                var validator = new CreateInvoiceRequestValidator(_customerService, _userService, _warrantyService, _productService);
                var result = await validator.ValidateAsync(createInvoiceRequest);
                if (!result.IsValid)
                {
                    //Add all error messages to an array
                    var errorMessages = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                    return BadRequest(errorMessages);
                }

                await _invoiceService.AddAsync(createInvoiceRequest);

                return Ok(createInvoiceRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Add Purchase Invoice
        /// <summary>
        /// Add an purchase invoice in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "customerName": "Tran Thi A",
        ///       "phoneNumber": "0912345789",
        ///       "inOrOut": "In",
        ///       "userId": 1,
        ///       "perDiscount": 0,
        ///       "invoiceDetails": [
        ///         1,
        ///         2
        ///       ]
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Invoice that was created</returns>
        /// <response code="200">Invoice that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost("CreatePurchaseInvoice")]
        public async Task<IActionResult> AddPurchaseInvoiceAsync([FromBody] CreatePurchaseInvoiceRequest createPurchaseInvoiceRequest)
        {
            try
            {
                await _invoiceService.AddPurchaseInvoiceAsync(createPurchaseInvoiceRequest);
                return Ok(createPurchaseInvoiceRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Invoice By Id
        /// <summary>
        /// Get an invoice in the system
        /// </summary>
        /// <param name="id">Id of the invoice you want to get</param>
        /// <returns>An user</returns>
        /// <response code="200">Return an invoice in the system</response>
        /// <response code="400">If the invoice is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _invoiceService.GetByIdWithIncludeAsync(id);

                if (result is not null)
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return NotFound(new
            {
                ErrorMessage = $"Invoice with {id} does not exist"
            });
        }
        #endregion

        #region Invoice Checkout
        /// <summary>
        /// Check out an invoice in the system
        /// </summary>
        /// <param name="id">Id of the invoice you want to pay</param>
        /// <returns>An user</returns>
        /// <response code="200">Return an url to VnPay</response>
        /// <response code="400">If the invoice is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost("{id}/payment")]
        public async Task<IActionResult> CreateAsync(int id)
        {
            var url = await _vnPayService.CreateUrl(id);

            return Ok(new
            {
                PaymentUrl = url
            });
        }
        #endregion

        #region Update Invoice
        /// <summary>
        /// Update an invoice in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "invoiceId": 1,
        ///       "invoiceStatus": "Pending",
        ///       "customerName": "Tran Thi A",
        ///       "userId": 1,
        ///       "total": 15000000,
        ///       "perDiscount": 10,
        ///       "invoiceDetails": [
        ///         {
        ///           "productId": 4,
        ///           "quantity": 1
        ///         },
        ///         {
        ///           "productId": 6,
        ///           "quantity": 2
        ///         }
        ///       ]
        ///     }
        ///         
        /// </remarks> 
        /// <returns>No content</returns>
        /// <response code="204">No content</response>
        /// <response code="400">Invoice does not exist</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateInvoiceRequest invoice)
        {
            try
            {
                //Use Fluent Validation
                var validator = new UpdateInvoiceRequestValidator(_customerService, _userService, _warrantyService, _productService, _invoiceService);
                var result = await validator.ValidateAsync(invoice);
                if (!result.IsValid)
                {
                    //Add all error messages to an array
                    var errorMessages = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                    return BadRequest(errorMessages);
                }
                await _invoiceService.UpdateAsync(invoice);

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete Invoice
        /// <summary>
        /// Change the invoice status in the system
        /// </summary>
        /// <param name="id">Id of the invoice you want to change</param>
        /// <response code="204">No Content</response>
        /// <response code="400">If the invoice is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                var result = await _invoiceService.GetByIdAsync(id);

                if (result == null) return NotFound(new
                {
                    ErrorMessage = $"Invoice with {id} does not exist"
                });

                await _invoiceService.DeleteInvoice(id);

                return Ok("Update Successfully!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Change Invoice Status
        /// <summary>
        /// Change an invoice status
        /// </summary>
        /// <param name="id">Id of the invoice you want to change</param>
        /// <returns>An user</returns>
        /// <response code="204">No content</response>
        /// <response code="400">If the invoice is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut("{id}/change")]
        public async Task<IActionResult> ChangeInvoiceStatus(int id)
        {
            try
            {
                var result = await _invoiceService.GetByIdAsync(id);

                if (result is not null)
                {
                    await _invoiceService.ChangeInvoiceStatus(id);
                    return Ok("Update Successfully!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return NotFound(new
            {
                ErrorMessage = $"Invoice with {id} does not exist"
            });
        }
        #endregion

        #region Cancel Invoice
        /// <summary>
        /// Cancel an invoice
        /// </summary>
        /// <param name="id">Id of the invoice you want to cancel</param>
        /// <returns>An user</returns>
        /// <response code="204">No content</response>
        /// <response code="400">If the invoice is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelInvoice(int id)
        {
            try
            {
                var result = await _invoiceService.GetByIdAsync(id);

                if (result is not null)
                {
                    await _invoiceService.CancelInvoice(id);
                    return Ok("Update Successfully!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return NotFound(new
            {
                ErrorMessage = $"Invoice with {id} does not exist"
            });
        }
        #endregion

        #region Generate Invoice to Pdf
        /// <summary>
        /// Generate Invoice to Pdf
        /// </summary>
        /// <param name="invoiceId">InvoiceId you want to export to PDF file</param>
        /// <returns>Invoice PDF file</returns>
        [HttpGet("{invoiceId}/pdf")]
        public async Task<IActionResult> GetInvoicePdf(int invoiceId)
        {
            var pdfBytes = await _invoiceService.GenerateInvoicePdf(invoiceId);

            // Return PDF as File Content
            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }
        #endregion

        #region  Generate Invoice to Excel
        /// <summary>
        /// Generate Invoice to Excel
        /// </summary>
        /// <param name="month">Month you want to export to excel file</param>
        /// <param name="year">Year you want to export to excel file</param>
        /// <returns>Excel file has all sales invoices for a month</returns>
        [HttpGet("report")]
        public async Task<IActionResult> GetInvoiceReport(int month, int year)
        {
            var excelFile = await _invoiceService.GenerateInvoiceExcel(month, year);

            if (excelFile == null)
            {
                return BadRequest("Invalid month or year.");
            }

            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"InvoiceListFor_{month}_{year}.xlsx");
        }
        #endregion

        #region Monthly Profit
        /// <summary>
        ///Get Monthly Revenue
        /// </summary>
        /// <returns>Monthly Revenue</returns>
        /// 
        [HttpGet("monthlyProfit")]
        public async Task<IActionResult> GetMonthlyRevenue()
        {
            try
            {
                var currentDate = DateTime.Now;
                var month = currentDate.Month;
                var year = currentDate.Year;

                var revenue = await _invoiceService.GetMonthlyRevenueAsync(month,year);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Number Of Transactions
        /// <summary>
        ///Get Number of Transactions
        /// </summary>
        /// <returns>Number of transactions</returns>
        [HttpGet("transaction")]
        public async Task<IActionResult> GetTransactionCount()
        {
            try
            {
                var currentDate = DateTime.Now;
                var month = currentDate.Month;
                var year = currentDate.Year;

                var transactionCount = await _invoiceService.GetTransactionCountAsync(month, year);
                return Ok(transactionCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Daily Profit
        /// <summary>
        ///Get Daily Revenue
        /// </summary>
        /// <returns>Daily Revenue</returns>
        /// 
        [HttpGet("dailyProfit")]
        public async Task<IActionResult> GetDailyRevenue()
        {
            try
            {
                var currentDate = DateTime.Now;
                var day = currentDate.Day;
                var month = currentDate.Month;
                var year = currentDate.Year;

                var revenue = await _invoiceService.GetDailyRevenueAsync(day,month, year);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Monthly Profit Change
        [HttpGet("monthlyProfitChange")]
        public async Task<IActionResult> GetMonthlyProfitChange()
        {
            try
            {
                var profitChange = await _invoiceService.GetMonthlyProfitChangeAsync();
                return Ok(profitChange);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the monthly profit change.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        #endregion

        #region List MonthRevenue of the Year for Admin
        /// <summary>
        ///Get List MonthRevenue of the Year for Admin
        /// </summary>
        /// <returns>Week Revenue</returns>
        [HttpGet("MonthlyRevenueOfYear")]
        public async Task<IActionResult> GetListMonthRevenueOfYear()
        {
            try
            {
                var currentDate = DateTime.Now;


                var revenue = await _invoiceService.GetRevenueForEachMonthAsync(currentDate);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region List QuantityProduct sale each Month of the Year for Admin
        /// <summary>
        ///Get List QuantityProduct sale each Month of the Year for Admin
        /// </summary>
        /// <returns>Week Revenue</returns>
        [HttpGet("QuantityProductSaleInMonth")]
        public async Task<IActionResult> GetLQuantityProductOfSaleInMonth()
        {
            try
            {
                var currentDate = DateTime.Now;


                var revenue = await _invoiceService.GetQuantiyProductForEachMonthAsync(currentDate);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Generate Warranty Of Invoices
        /// <summary>   
        /// Generate Warranty Card to Pdf
        /// </summary>
        /// <param name="invoiceId">InvoiceId you want to export to PDF file</param>
        /// <param name="warrantyId">Warranty you want to apply in invoice</param>
        /// <returns>Invoice PDF file</returns>
        [HttpGet("pdf")]
        public async Task<IActionResult> GetWarrantyOfInvoicePdf(int invoiceId, int warrantyId)
        {
            var pdfBytes = await _invoiceService.GenerateWarrantyInvoicePdf(invoiceId, warrantyId);

            // Return PDF as File Content
            return File(pdfBytes, "application/pdf", "invoiceWarranty.pdf");
        }
        #endregion
    }

}