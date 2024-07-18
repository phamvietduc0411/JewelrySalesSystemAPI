using JewelrySalesSystem.BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/colors")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly ILogger<ColorsController> _logger;
        private readonly IColorService _colorService;

        public ColorsController(
            ILogger<ColorsController> logger,
            IColorService colorService)
        {
            _logger = logger;
            _colorService = colorService;
        }

        #region Get All Colors
        /// <summary>
        /// Get all colors in the system
        /// </summary>
        /// <returns>A list of all colors</returns>
        /// <response code="200">Return all colors in the system</response>
        /// <response code="400">If no colors are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _colorService.GetAllAsync();

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

        //#region Get Colour By Id
        ///// <summary>
        ///// Get a color based on Id in the system
        ///// </summary>
        ///// <param name="id">Id of the color you want to get</param>
        ///// <returns>A colour</returns>
        ///// <response code="200">Return a color in the system</response>
        ///// <response code="400">If the color is null</response>
        ///// <response code="401">Unauthorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="404">Not Found</response>
        ///// <response code="500">Internal Server</response>
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        var result = await _colorService.GetByIdAsync(id);

        //        if (result is not null)
        //        {
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return NotFound(new
        //    {
        //        ErrorMessage = $"Color with {id} does not exist"
        //    });
        //}
        //#endregion
    }
}
