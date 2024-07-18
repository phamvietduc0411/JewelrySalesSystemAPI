using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/cuts")]
    [ApiController]
    public class CutsController : ControllerBase
    {
        private readonly ILogger<CutsController> _logger;
        private readonly ICutService _cutService;

        public CutsController(
            ILogger<CutsController> logger,
            ICutService cutService)
        {
            _logger = logger;
            _cutService = cutService;
        }

        #region Get All Cuts
        /// <summary>
        /// Get all cuts in the system
        /// </summary>
        /// <returns>A list of all cuts</returns>
        /// <response code="200">Return all cuts in the system</response>
        /// <response code="400">If no cuts are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _cutService.GetAllAsync();

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
