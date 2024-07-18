using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/caratweights")]
    [ApiController]
    public class CaratWeightsController : ControllerBase
    {
        private readonly ILogger<CaratWeightsController> _logger;
        private readonly ICaratService _caratService;

        public CaratWeightsController(
            ILogger<CaratWeightsController> logger,
            ICaratService caratService)
        {
            _logger = logger;
            _caratService = caratService;
        }

        #region Get All Carats
        /// <summary>
        /// Get all carat weights in the system
        /// </summary>
        /// <returns>A list of all carat weights</returns>
        /// <response code="200">Return all carat weights in the system</response>
        /// <response code="400">If no carat weights are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _caratService.GetAllAsync();

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
