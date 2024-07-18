using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/clarities")]
    [ApiController]
    public class ClaritiesController : ControllerBase
    {
        private readonly ILogger<ClaritiesController> _logger;
        private readonly IClarityService _clarityService;

        public ClaritiesController(
            ILogger<ClaritiesController> logger,
            IClarityService clarityService)
        {
            _logger = logger;
            _clarityService = clarityService;
        }

        #region Get All Claritys
        /// <summary>
        /// Get all claritys in the system
        /// </summary>
        /// <returns>A list of all clarities</returns>
        /// <response code="200">Return all clarities in the system</response>
        /// <response code="400">If no clarities are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _clarityService.GetAllAsync();

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
