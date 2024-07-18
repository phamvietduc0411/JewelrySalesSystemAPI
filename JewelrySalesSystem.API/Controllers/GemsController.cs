using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Gems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/gems")]
    [ApiController]
    public class GemsController : ControllerBase
    {
        private readonly ILogger<GemsController> _logger;
        private readonly IGemService _gemService;

        public GemsController(
            ILogger<GemsController> logger,
            IGemService gemService)
        {
            _logger = logger;
            _gemService = gemService;
        }

        #region Get All Gems
        /// <summary>
        /// Get all gems in the system
        /// </summary>
        /// <param name="page">Current page the user is on</param>
        /// <param name="pageSize">Number of entities you want to show</param>
        /// <param name="isActive">Gem active or not</param>
        /// <param name="searchTerm">Search query</param>
        /// <param name="sortColumn">Column you want to sort</param>
        /// <param name="sortOrder">Sort column by ascending or descening</param>
        /// <returns>A list of all gems</returns>
        /// <response code="200">Return all gems in the system</response>
        /// <response code="400">If no gems are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page = 1,
            int pageSize = 5)
        {
            try
            {
                var result = await _gemService.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize);

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

        #region Add Gem
        /// <summary>
        /// Add a gem in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "gemName": "Test Gem",
        ///       "featuredImage": "url",
        ///       "shapeId": 1,
        ///       "originId": 1,
        ///       "caratId": 1,
        ///       "colorId": 1,
        ///       "clarityId" : 1,
        ///       "cutId": 1,
        ///       "price": 0
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Gem that was created</returns>
        /// <response code="200">Gem that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateGemRequest createGemRequest)
        {
            try
            {
                await _gemService.AddAsync(createGemRequest);

                return Ok(createGemRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Gem By Id
        /// <summary>
        /// Get a gem in the system
        /// </summary>
        /// <param name="id">Id of the gem you want to get</param>
        /// <returns>A product</returns>
        /// <response code="200">Return a gem in the system</response>
        /// <response code="400">If the gem is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _gemService.GetByIdWithIncludeAsync(id);

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
                ErrorMessage = "Gem does not exist"
            });
        }
        #endregion

        #region Update Gem
        /// <summary>
        /// Update a gem in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "gemId": 1,
        ///       "gemName": "Test Gem",
        ///       "featuredImage": "url",
        ///       "shapeId": 1,
        ///       "originId": 1,
        ///       "caratId": 1,
        ///       "colorId": 1,
        ///       "clarityId" : 1,
        ///       "cutId": 1
        ///     }
        ///         
        /// </remarks> 
        /// <returns>No content</returns>
        /// <response code="204">No content</response>
        /// <response code="400">Gem does not exist</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateGemRequest updateGemRequest)
        {
            try
            {
                await _gemService.UpdateAsync(updateGemRequest);

                return Ok(updateGemRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Gem Price
        /// <summary>
        /// Get gem price in the system
        /// </summary>
        /// <returns>A product</returns>
        /// <response code="200">Return a gem in the system</response>
        /// <response code="400">If the gem is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("price")]
        public async Task<IActionResult> GetGemPriceAsync([FromQuery] GemPriceRequest gemPriceRequest)
        {
            try
            {
                var result = await _gemService.GetGemPriceAsync(gemPriceRequest);

                if (result > 0)
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
                ErrorMessage = "Gem Price does not exist"
            });
        }
        #endregion

        #region Gem All Gem Prices
        /// <summary>
        /// Get all gem prices in the system
        /// </summary>
        /// <returns>A list of all gem prices</returns>
        /// <response code="200">Return all gem prices in the system</response>
        /// <response code="400">If no gem prices are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("prices")]
        public async Task<IActionResult> GetGemPricesAsync()
        {
            try
            {
                var result = await _gemService.GetGemPricesAsync();

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

        #region Delete Gem
        /// <summary>
        /// Change the gem status in the system
        /// </summary>
        /// <param name="id">Id of the gem you want to change</param>
        /// <response code="200">Ok</response>
        /// <response code="400">If the product is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _gemService.DeleteAsync(id);              

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
    }
}
