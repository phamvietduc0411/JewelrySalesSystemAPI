using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/origins")]
    [ApiController]
    public class OriginsController : ControllerBase
    {
        private readonly ILogger<OriginsController> _logger;
        private readonly IOriginService _originService;

        public OriginsController(
            ILogger<OriginsController> logger,
            IOriginService originService)
        {
            _logger = logger;
            _originService = originService;
        }

        #region Get All Origins
        /// <summary>
        /// Get all origins in the system
        /// </summary>
        /// <returns>A list of all origins</returns>
        /// <response code="200">Return all origins in the system</response>
        /// <response code="400">If no origins are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _originService.GetAllAsync();

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
