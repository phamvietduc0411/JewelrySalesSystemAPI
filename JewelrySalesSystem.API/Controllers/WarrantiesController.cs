using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Users;
using JewelrySalesSystem.BAL.Models.Warranties;
using JewelrySalesSystem.BAL.Services;
using JewelrySalesSystem.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/warranties")]
    [ApiController]
    public class WarrantiesController : ControllerBase
    {
        private readonly ILogger<WarrantiesController> _logger;
        private readonly IWarrantyService _warrantyService;

        public WarrantiesController(
            ILogger<WarrantiesController> logger,
            IWarrantyService warrantyService)
        {
            _logger = logger;
            _warrantyService = warrantyService;
        }

        #region Get All Warranties
        /// <summary>
        /// Get all warranties in the system.
        /// </summary>
        /// <param name="page">Current page of warranties to retrieve.</param>
        /// <param name="pageSize">Number of warranties to show per page.</param>
        /// <param name="searchTerm">Search query to filter the warranties.</param>
        /// <param name="sortColumn">Column by which to sort the warranties.</param>
        /// <param name="sortOrder">Order in which to sort the column, either ascending or descending.</param>
        /// <returns>A list of all warranties in the system.</returns>
        /// <response code="200">Returns all warranties in the system.</response>
        /// <response code="400">If no warranties are found in the system.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Not Found.</response>
        /// <response code="500">Internal Server Error.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page = 1,
            int pageSize = 5)
        {
            try
            {
                var result = await _warrantyService.PaginationAsync(searchTerm, sortColumn, sortOrder, page, pageSize);

                if (result is not null)
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

        #region Get Warranty By Id
        /// <summary>
        /// Get a warranty 
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _warrantyService.GetWarrantyById(id);
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
                ErrorMessage = "Warranty does not exist"
            });
        }
        #endregion

        #region Update Warranty
        /// <summary>
        /// Update New Warranty
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateWarrantyRequest updateWarrantyRequest)
        {
            try
            {
                //var warranty = await _warrantyService.GetById(updateWarrantyRequest.WarrantyId);
                //if (warranty == null)
                //    return NotFound("Warranty not found.");

                await _warrantyService.UpdateAsync(updateWarrantyRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create New Warranty
        /// <summary>
        /// Create New Warranty
        /// </summary>
        /// <param name="warranty"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsysc(CreateWarrantyRequest warranty)
        {
            try
            {
                await _warrantyService.AddNewWarranty(warranty);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
