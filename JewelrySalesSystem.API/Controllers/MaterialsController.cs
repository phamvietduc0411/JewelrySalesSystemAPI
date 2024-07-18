using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.MaterialPriceList;
using JewelrySalesSystem.BAL.Models.Materials;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ILogger<MaterialsController> _logger;
        private readonly IMaterialService _materialService;
        private readonly IProductService _productService;
        private readonly IMaterialPriceListService _materialPriceListService;

        public MaterialsController(
            ILogger<MaterialsController> logger,
            IMaterialService materialService,
            IProductService productService,
            IMaterialPriceListService materialPriceListService)
        {
            _logger = logger;
            _materialService = materialService;
            _materialPriceListService = materialPriceListService;
            _productService = productService;
        }

        #region Get All Materials
        /// <summary>
        /// Get all materials in the system
        /// </summary>
        /// <param name="page">Current page the user is on</param>
        /// <param name="pageSize">Number of entities you want to show</param>
        /// <param name="isActive">Material active or not</param>
        /// <param name="searchTerm">Search query</param>
        /// <param name="sortColumn">Column you want to sort</param>
        /// <param name="sortOrder">Sort column by ascending or descening</param>
        /// <returns>A list of all users</returns>
        /// <response code="200">Return all materials in the system</response>
        /// <response code="400">If no materials are in the system</response>
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
                var result = await _materialService.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize);

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

        //#region Get All Gold Materials
        ///// <summary>
        ///// Get all gold materials in the system
        ///// </summary>
        ///// <returns>A list of gold materials</returns>
        ///// <response code="200">Return a list of gold materials in the system</response>
        ///// <response code="400">If the list is null</response>
        ///// <response code="401">Unauthorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="404">Not Found</response>
        ///// <response code="500">Internal Server</response>
        //[HttpGet("golds")]
        //public async Task<IActionResult> GetAllGoldMaterials()
        //{
        //    try
        //    {
        //        var result = await _materialService.GetAllGoldMaterials();

        //        if (result is not null)
        //        {
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return NotFound();
        //}
        //#endregion

        #region Add Material
        /// <summary>
        /// Add a material in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "materialName": "Vàng 16K",
        ///       "materialPrice": {
        ///          "buyPrice": 11,
        ///          "sellPrice": 11
        ///        }            
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Material that was created</returns>
        /// <response code="200">Material that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateMaterialRequest createMaterialRequest)
        {
            try
            {
                await _materialService.AddAsync(createMaterialRequest);

                return Ok(createMaterialRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Add Material Price
        /// <summary>
        /// Add new material prices in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     [
        ///       {
        ///         "buyPrice": 7600000,
        ///         "sellPrice": 7800000,
        ///         "materialId": 1
        ///       },
        ///       {
        ///         "buyPrice": 7600000,
        ///         "sellPrice": 7800000,
        ///         "materialId": 2
        ///       },
        ///       {
        ///         "buyPrice": 7600000,
        ///         "sellPrice": 7800000,
        ///         "materialId": 3
        ///       },
        ///       {
        ///         "buyPrice": 7600000,
        ///         "sellPrice": 7800000,
        ///         "materialId": 4
        ///       },
        ///       {
        ///         "buyPrice": 7600000,
        ///         "sellPrice": 7800000,
        ///         "materialId": 5
        ///       },
        ///       {
        ///         "buyPrice": 7600000,
        ///         "sellPrice": 7800000,
        ///         "materialId": 6
        ///       }
        ///     ]
        ///         
        /// </remarks> 
        /// <returns>Material prices that was created</returns>
        /// <response code="200">Material that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost("price")]
        public async Task<IActionResult> AddAsync([FromBody] List<CreateMaterialPriceList> prices)
        {
            try
            {
                //var material = await _materialService.GetByIdAsync(id);

                //if (material == null) return NotFound(new
                //{
                //    ErrorMessage = $"Material with {id} does not exist"
                //});

                var result = await _materialPriceListService.AddAsync(prices);

                await _productService.UpdateProductPriceAsync();

                return Ok(prices);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Material By Id
        /// <summary>
        /// Get a material in the system
        /// </summary>
        /// <param name="id">Id of the material you want to get</param>
        /// <returns>A product</returns>
        /// <response code="200">Return a material in the system</response>
        /// <response code="400">If the material is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _materialService.GetByIdWithIncludeAsync(id);

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
                ErrorMessage = $"Material with {id} does not exist"
            });
        }
        #endregion

        #region Update Material
        /// <summary>
        /// Update a material in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "materialId" : 5,
        ///       "materialName": "Gold 10K"
        ///     }
        ///         
        /// </remarks> 
        /// <returns>No content</returns>
        /// <response code="204">No content</response>
        /// <response code="400">Material does not exist</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateMaterialRequest updateMaterialRequest)
        {
            try
            {
                var result = await _materialService.GetByIdAsync(updateMaterialRequest.MaterialId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        ErrorMessage = $"Material with {updateMaterialRequest.MaterialId} does not exist"
                    });
                }
                else if (updateMaterialRequest.MaterialName.Trim().Equals(result.MaterialName.Trim()))
                {
                    return BadRequest(new
                    {
                        ErrorMessage = $"Material with {updateMaterialRequest.MaterialName} has already existed"
                    });
                }

                await _materialService.UpdateAsync(updateMaterialRequest);

                return Ok(updateMaterialRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete Material
        /// <summary>
        /// Change the material status in the system
        /// </summary>
        /// <param name="id">Id of the material you want to change</param>
        /// <response code="204">No Content</response>
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
                var result = await _materialService.GetByIdAsync(id);

                if (result == null) return NotFound(new
                {
                    ErrorMessage = $"Material with {id} does not exist"
                });

                await _materialService.DeleteAsync(id);

                return Ok("Update Successfully!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
