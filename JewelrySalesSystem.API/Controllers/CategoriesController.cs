using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        #region Gell All Categories
        /// <summary>
        /// Get all categories in the system
        /// </summary>
        /// <returns>A list of all categories</returns>
        /// <response code="200">Return all categories in the system</response>
        /// <response code="400">If no categories are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _categoryService.GetAllAsync();

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

        #region Get All Products By Category Id
        /// <summary>
        /// Get all products by category Id in the system
        /// </summary>
        /// <returns>A list of all products</returns>
        /// <response code="200">Return all products that have categoryId in the system</response>
        /// <response code="400">If no products are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetAllProductsByCategoryIdAsync(int id)
        {
            try
            {
                var result = await _categoryService.GetAllProductsByCategoryIdAsync(id);

                if (result is not null)
                {
                    return Ok(result.Products);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return NotFound(new
            {
                ErrorMessage = $"Category with {id} does not exist"
            });
        }
        #endregion

        #region Get Category By Id
        /// <summary>
        /// Get a category based on Id in the system
        /// </summary>
        /// <param name="id">Id of the category you want to get</param>
        /// <returns>A category</returns>
        /// <response code="200">Return a category in the system</response>
        /// <response code="400">If the category is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);

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
                ErrorMessage = $"Category with {id} does not exist"
            });
        }
        #endregion

        #region Update Category
        /// <summary>
        /// Update a category in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "categoryId": 1,
        ///       "categoryName": "Updated Category"
        ///     }
        ///         
        /// </remarks> 
        /// <returns>No content</returns>
        /// <response code="204">No content</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(updateCategoryRequest.CategoryId);

                if (result == null) return NotFound(new
                {
                    ErrorMessage = $"Category with {updateCategoryRequest.CategoryId} does not exist"
                });

                await _categoryService.UpdateAsync(updateCategoryRequest);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Add Category
        /// <summary>
        /// Add a category in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "categoryName": "Test Category"
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Category that was created</returns>
        /// <response code="200">Category that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            try
            {
                await _categoryService.AddNewCategory(createCategoryRequest);

                return Ok(createCategoryRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
