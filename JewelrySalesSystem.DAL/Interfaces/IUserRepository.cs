using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> LoginAsync(string userName, string passWord);

        Task<PaginatedList<User>> PaginationAsync
            (string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize);

        Task<List<User>> GetUsersWithRoleSeller(int roleId);

        Task<List<User>> GetUsersWithRoleCashier(int roleId);

        Task<User?> GetByIdWithIncludeAsync(int id);

        Task DeleteAsync(int id);

        Task<User> CheckDuplicate(string userName , string option);

        Task<bool> CheckRoleIdExists(int roleId);

        Task AssignUserToCounter(int userId, int counterId);
    }
}
