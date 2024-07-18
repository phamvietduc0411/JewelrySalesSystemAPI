using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/shapes")]
    [ApiController]
    public class ShapesController : ControllerBase
    {
        private readonly ILogger<ShapesController> _logger;
        private readonly IShapeService _shapeService;

        public ShapesController(
            ILogger<ShapesController> logger,
            IShapeService shapeService)
        {
            _logger = logger;
            _shapeService = shapeService;
        }

        #region Get All Shapes
        /// <summary>
        /// Get all shapes in the system
        /// </summary>
        /// <returns>A list of all shapes</returns>
        /// <response code="200">Return all shapes in the system</response>
        /// <response code="400">If no shapes are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _shapeService.GetAllAsync();

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
