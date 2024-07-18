using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/countertypes")]
    [ApiController]
    public class CounterTypesController : ControllerBase
    {
        private readonly ILogger<CounterTypesController> _logger;
        private readonly ICounterTypeService _counterTypeService;

        public CounterTypesController(
            ILogger<CounterTypesController> logger,
            ICounterTypeService counterTypeService)
        {
            _logger = logger;
            _counterTypeService = counterTypeService;
        }

        #region Get All Counter Types
        /// <summary>
        /// Get all counter types in the system
        /// </summary>
        /// <returns>A list of all counter types</returns>
        /// <response code="200">Return all counter types in the system</response>
        /// <response code="400">If no counter types are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _counterTypeService.GetAllAsync();

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
