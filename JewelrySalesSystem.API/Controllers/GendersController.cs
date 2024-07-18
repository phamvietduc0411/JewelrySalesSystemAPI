using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Genders;
using JewelrySalesSystem.BAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/genders")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly ILogger<GendersController> _logger;
        private readonly IGenderService _genderService;

        public GendersController(
            ILogger<GendersController> logger,
            IGenderService genderService)
        {
            _logger = logger;
            _genderService = genderService;
        }

        #region Add Gender
        /// <summary>
        /// Add a gender in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "genderName": "Test Gender"
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Gender that was created</returns>
        /// <response code="200">Gender that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddGender(CreateGenderRequest createGenderRequest)
        {
            try
            {
                var result = await _genderService.AddGender(createGenderRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get All Genders
        /// <summary>
        /// Get all genders in the system
        /// </summary>
        /// <returns>A list of all genders</returns>
        /// <response code="200">Return all genders in the system</response>
        /// <response code="400">If no genders are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _genderService.GetAllAsync();

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

        #region Get Gender By Id
        /// <summary>
        /// Get a gender based on Id in the system
        /// </summary>
        /// <param name="id">Id of the gender you want to get</param>
        /// <returns>A gender</returns>
        /// <response code="200">Return a gender in the system</response>
        /// <response code="400">If the gender is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _genderService.GetByIdAsync(id);

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
                ErrorMessage = $"Gender with {id} does not exist"
            });
        }
        #endregion
    }
}
