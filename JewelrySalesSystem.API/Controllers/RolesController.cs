using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Categories;
using JewelrySalesSystem.BAL.Models.Roles;
using JewelrySalesSystem.BAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleService _roleService;

        public RolesController(
            ILogger<RolesController> logger,
            IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;

        }

        #region Add Role
        /// <summary>
        /// Add a role in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "roleName": "Test Role"
        ///     }
        ///         
        /// </remarks> 
        /// <returns>Role that was created</returns>
        /// <response code="200">Role that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateRoleRequest createRoleRequest)
        {
            try
            {
                var result = await _roleService.AddRoleAsync(createRoleRequest);

                return Ok(createRoleRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get All Role
        /// <summary>
        /// Get role in the system
        /// </summary>
        [HttpGet("getAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            try
            {
                var result = await _roleService.GetAllAsync();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
               throw new Exception($"{ex.Message}");
            }
        }
        #endregion
    }


}

