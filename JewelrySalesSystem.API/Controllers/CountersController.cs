using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Counters;
using JewelrySalesSystem.DAL.Entities;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PdfSharpCore;
using System.Drawing.Printing;
using ZXing;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/counters")]
    [ApiController]
    public class CountersController : ControllerBase
    {
        private readonly ILogger<CountersController> _logger;
        private readonly ICounterService _counterService;

        public CountersController(
            ILogger<CountersController> logger,
            ICounterService counterService)
        {
            _logger = logger;
            _counterService = counterService;
        }

        #region Get All Counters
        /// <summary>
        /// Get all counters in the system
        /// </summary>
        /// <param name="isActive">Counter active or not</param>
        /// <param name="page">Current page the counter is on</param>
        /// <param name="pageSize">Number of entities you want to show</param>
        /// <param name="searchTerm">Search query</param>
        /// <param name="sortColumn">Column you want to sort</param>
        /// <param name="sortOrder">Sort column by ascending or descening</param>
        /// <returns>A list of all counters</returns>
        /// <response code="200">Return all counters in the system</response>
        /// <response code="400">If no counters are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page = 1,
            int pageSize = 5)
        {
            try
            {
                var result = await _counterService.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize);

                if (result is not null)
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return NotFound();
        }
        #endregion

        #region Add Counter
        /// <summary>
        /// Add a counter in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "counterName": "I",
        ///       "counterTypeId": 1
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Counter that was created</returns>
        /// <response code="200">Counter that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateCounterRequest request)
        {
            try
            {
                await _counterService.AddAsync(request);

                return Ok(request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Counter
        /// <summary>
        /// Update an user in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "counterId" : 1,
        ///       "counterName": "X",
        ///       "counterTypeId" : 2
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Ok</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">User does not exist</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCounterRequest request)
        {
            try
            {
                await _counterService.UpdateAsync(request);

                return Ok("Update Successfully!");
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region Delete Counter
        /// <summary>
        /// Change the counter status in the system
        /// </summary>
        /// <param name="id">Id of the counter you want to change</param>
        /// <returns>A product</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the user is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _counterService.ChangeStatusAsync(id);

                return Ok("Update Successfully!");
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region Assign User to Counter
        /// <summary>
        /// Assign an user to a counter in the system
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="counterId">Counter Id</param>
        /// <returns>No content</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">User does not exist</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut("{counterId}/assign-to-user")]
        public async Task<IActionResult> AssignToCounter(int counterId, int userId)
        {
            try
            {
                await _counterService.AssignStaffToCounterAsync(counterId, userId);

                return Ok("Assign successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region Unassign Counter
        /// <summary>
        /// Unassign a counter
        /// </summary>
        /// <param name="id">Id of the counter you want to change</param>
        /// <returns>A product</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the user is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut("{id}/unassign")]
        public async Task<IActionResult> UnassignCounterAsync(int id)
        {
            try
            {
                await _counterService.UnassignCounterAsync(id);

                return Ok("Update Successfully!");
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    ErrorMessage = ex.Message
                });
            }
        }
        #endregion

        #region
        /// <summary>
        /// Get all counters in the system including counter id and name
        /// </summary>
        [HttpGet("id-name")]
        public async Task<ActionResult<List<GetAllCounterName>>> GetAllCounterIdAndNames()
        {
            try
            {
                var result = await _counterService.GetAllCounterIdAndNamesAsync();
                if (result is not null)
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return NotFound();
        }

        #endregion
    }
}
