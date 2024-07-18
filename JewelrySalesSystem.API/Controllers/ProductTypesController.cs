using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.ProductTypes;
using JewelrySalesSystem.BAL.Models.Users;
using JewelrySalesSystem.BAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/producttypes")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
        private readonly ILogger<ProductTypesController> _logger;
        private readonly IProductTypeService _productTypeService;

        public ProductTypesController(
            ILogger<ProductTypesController> logger,
            IProductTypeService productTypeService)
        {
            _logger = logger;
            _productTypeService = productTypeService;
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetAllProductsByProductTypeIdAsync(int id)
        {
            try
            {
                var result = await _productTypeService.GetAllProductsByProductTypeIdAsync(id);
                if (result != null)
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

        #region Get All Product Types
        /// <summary>
        /// Get all product types in the system
        /// </summary>
        /// <returns>A list of all product types</returns>
        /// <response code="200">Return all product types in the system</response>
        /// <response code="400">If no product types are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _productTypeService.GetAllAsync();

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

        #region Get Product Type By Id
        /// <summary>
        /// Get a product type based on Id in the system
        /// </summary>
        /// <param name="id">Id of the product type you want to get</param>
        /// <returns>A product type</returns>
        /// <response code="200">Return a product type in the system</response>
        /// <response code="400">If the product type is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _productTypeService.GetByIdAsync(id);

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
                ErrorMessage = $"Product Type with {id} does not exist"
            });
        }
        #endregion
        #region Add Product Type
        /// <summary>
        /// Add New Product Type
        /// </summary>
        /// 
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "name": "string"
        ///     }
        /// </remarks>
        /// <param name="productType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateProductTypeRequest productType)
        {
            try
            {
                var result = await _productTypeService.AddAsync(productType);
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update ProductType
        /// <summary>
        /// Update Product Type in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///     "id": 0,
        ///     "name": "string"
        ///     }
        /// </remarks>
        /// <param name="productType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UpdateTypeRequest productType)
        {
            try
            {
                var typeId = await _productTypeService.GetByIdAsync(productType.Id);
                if (typeId == null)
                    return BadRequest();

                await _productTypeService.UpdateAsync(productType);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
