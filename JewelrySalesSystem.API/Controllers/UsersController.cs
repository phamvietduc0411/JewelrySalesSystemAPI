using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelrySalesSystem.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(
            ILogger<UsersController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        #region Get All Users
        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <param name="isActive">User active or not</param>
        /// <param name="page">Current page the user is on</param>
        /// <param name="pageSize">Number of entities you want to show</param>
        /// <param name="searchTerm">Search query</param>
        /// <param name="sortColumn">Column you want to sort</param>
        /// <param name="sortOrder">Sort column by ascending or descening</param>
        /// <returns>A list of all users</returns>
        /// <response code="200">Return all users in the system</response>
        /// <response code="400">If no users are in the system</response>
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
                var result = await _userService.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize);

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

        #region Get All Sellers
        /// <summary>
        /// Get all unassigned sellers in the system
        /// </summary>
        /// <param name="roleId">User role</param>
        /// <returns>A list of all sellers</returns>
        /// <response code="200">Return all sellers in the system</response>
        /// <response code="400">If no sellers are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("roles/seller")]
        public async Task<IActionResult> GetAllSellersAsync(int roleId = 5)
        {
            try
            {
                var result = await _userService.GetUsersWithRoleSeller(roleId);

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

        #region Get All Cashiers
        /// <summary>
        /// Get all unassigned cashiers in the system
        /// </summary>
        /// <param name="roleId">User role</param>
        /// <returns>A list of all cashiers</returns>
        /// <response code="200">Return all cashiers in the system</response>
        /// <response code="400">If no cashiers are in the system</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("roles/cashier")]
        public async Task<IActionResult> GetAllCashiersAsync(int roleId = 4)
        {
            try
            {
                var result = await _userService.GetUsersWithRoleCashier(roleId);

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

        #region Add User
        /// <summary>
        /// Add an user in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "userName": "testaccount",
        ///       "fullName": "Nguyen Van B",
        ///       "phoneNumber": "0999123456",
        ///       "email": "testemail@gmail.com",
        ///       "password": "test",
        ///       "address": "test",
        ///       "roleId": 4
        ///     }
        ///         
        /// </remarks> 
        /// <returns>User that was created</returns>
        /// <response code="200">User that was created</response>
        /// <response code="400">Failed validation</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var result = await _userService.AddAsync(createUserRequest);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get User By Id
        /// <summary>
        /// Get an user in the system
        /// </summary>
        /// <param name="id">Id of the user you want to get</param>
        /// <returns>An user</returns>
        /// <response code="200">Return an user in the system</response>
        /// <response code="400">If the user is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _userService.GetByIdWithIncludeAsync(id);

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
                ErrorMessage = "User does not exist"
            });
        }
        #endregion

        #region Update User
        /// <summary>
        /// Update an user in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "userId" : 2,
        ///       "userName": "newtestaccount",
        ///       "fullName": "Nguyen Van C",
        ///       "phoneNumber": "0999123456",
        ///       "email": "testemail@gmail.com",
        ///       "password" : "test",
        ///       "address" : "test",
        ///       "roleId" : 2
        ///     }
        ///         
        /// </remarks> 
        /// <returns>No content</returns>
        /// <response code="204">No content</response>
        /// <response code="400">User does not exist</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequest updateUserRequest)
        {
            try
            {
                var user = await _userService.GetByIdWithIncludeAsync(updateUserRequest.UserId);

                if (user == null)
                    return BadRequest();

                await _userService.UpdateAsync(updateUserRequest);

                return Ok(updateUserRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete User
        /// <summary>
        /// Change the user status in the system
        /// </summary>
        /// <param name="id">Id of the user you want to change</param>
        /// <returns>A product</returns>
        /// <response code="204">No Content</response>
        /// <response code="400">If the user is null</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);

                if (result == null) return NotFound(new
                {
                    ErrorMessage = $"User with {id} does not exist"
                });

                await _userService.DeleteAsync(id);

                return Ok("Update Successfully!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //#region Assign User to Counter
        ///// <summary>
        ///// Assign an user to a counter in the system
        ///// </summary>
        ///// <param name="userId">User Id</param>
        ///// <param name="counterId">Counter Id</param>
        ///// <returns>No content</returns>
        ///// <response code="204">No content</response>
        ///// <response code="400">User does not exist</response>
        ///// <response code="401">Unauthorized</response>
        ///// <response code="403">Forbidden</response>
        ///// <response code="404">Not Found</response>
        ///// <response code="500">Internal Server</response>
        //[HttpPut("{userId}/assign-to-counter")]
        //public async Task<IActionResult> AssignToCounter(int userId, int counterId)
        //{
        //    try
        //    {
        //        await _userService.AssignUserToCounter(userId, counterId);

        //        return Ok(new
        //        {
        //            Message = "Assign successfully."
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new
        //        {
        //            ErrorMessage = ex.Message
        //        });
        //    }
        //}
        //#endregion

        #region Revenue Of Each Employee.
        /// <summary>
        /// Get revenue of each employee.
        /// </summary>
        [HttpGet("employee-revenue")]
        public async Task<IActionResult> GetEmployeeRevenue()
        {
            try
            {
                var currentDate = DateTime.Now;
                var month = currentDate.Month;
                var year = currentDate.Year;
                var excelFile = await _userService.GetEmployeeRevenue(month, year);
                if (excelFile == null || excelFile.Length == 0)
                {
                    return BadRequest("Invalid month or year.");
                }

                return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"EmployeeRevenue_{month}_{year}.xlsx");
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }
        #endregion
    }

}
