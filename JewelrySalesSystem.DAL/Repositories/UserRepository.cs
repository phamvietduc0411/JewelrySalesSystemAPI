using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(
            JewelryDbContext context,
            ILogger logger
            ) : base(context, logger)
        {
        }

        public async Task<User?> LoginAsync(string userName, string passWord)
        {
            try
            {
                var account = await _dbSet.Where(a => a.UserName == userName
                                                 && a.Password == passWord)
                                          .Include(u => u.Role)
                                          .FirstOrDefaultAsync();

                if (account != null)
                    return account;

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} LoginAsync method error", typeof(UserRepository));
                return new User();
            }
        }

        public async Task<PaginatedList<User>> PaginationAsync(
            string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize)
        {
            IQueryable<User> usersQuery = _dbSet
                .Include(u => u.Role)
                .Include(u => u.Counter);

            if (isActive) usersQuery = usersQuery.Where(u => u.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                usersQuery = usersQuery.Where(c =>
                    c.FullName.Contains(searchTerm) ||
                    c.PhoneNumber.Contains(searchTerm) ||
                    c.Email.Contains(searchTerm));
            }

            if (sortOrder?.ToLower() == "asc")
            {
                usersQuery = usersQuery.OrderBy(GetSortProperty(sortColumn));
            }
            else
            {
                usersQuery = usersQuery.OrderByDescending(GetSortProperty(sortColumn));
            }

            var users = await PaginatedList<User>.CreateAsync(usersQuery, page, pageSize);

            return users;
        }

        private static Expression<Func<User, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "name" => user => user.FullName,
            //"dob" => user => user.DoB,
            _ => user => user.UserId
        };

        public async Task<User?> GetByIdWithIncludeAsync(int id)
        {
            var result = await _dbSet.Include(u => u.Role).Include(u => u.Counter)
                               .FirstOrDefaultAsync(u => u.UserId == id);

            if (result == null) { return null; }

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var checkExistUser = await _dbSet.FindAsync(id);

            if (checkExistUser == null)
            {
                throw new Exception($"User with {id} does not exist");
            }
            else
            {
                if (checkExistUser.IsActive)
                {
                    checkExistUser.IsActive = false;
                }
                else
                {
                    checkExistUser.IsActive = true;
                }
                _dbSet.Update(checkExistUser);
            }
        }

        public async Task<User> CheckDuplicate(string detail, string option)
        {

            switch (option)
            {
                case "userName":
                    return await _dbSet.FirstOrDefaultAsync(u => u.UserName == detail);
                case "phoneNumber":
                    return await _dbSet.FirstOrDefaultAsync(u => u.PhoneNumber == detail);
                case "email":
                    return await _dbSet.FirstOrDefaultAsync(u => u.Email == detail);
                default:
                    throw new ArgumentException("Invalid option provided.");
            }
        }

        public async Task<bool> CheckRoleIdExists(int roleId)
        {
            return await _dbSet.AnyAsync(u => u.RoleId == roleId);
        }

        public async Task AssignUserToCounter(int userId, int counterId)
        {
            var result = await _dbSet.FindAsync(userId) ?? throw new Exception($"User with {userId} does not exist");

            var counter = await _context.Counters.FindAsync(counterId) ?? throw new Exception($"Counter with {counterId} does not exist");

            if (result.RoleId == 1 || result.RoleId == 2)
            {
                throw new Exception($"Can not assign administator or manager to a counter");
            }

            if (result.CounterId == null)
            {
                result.CounterId = counterId;
                _dbSet.Update(result);
            }
            else if (result.CounterId == counterId)
            {
                throw new Exception($"User: {userId} has already been assigned to counter: {counterId}");
            }
            else
            {
                result.CounterId = counterId;
                _dbSet.Update(result);
            }
        }

        public async Task<List<User>> GetUsersWithRoleSeller(int roleId)
            => await _dbSet
                     .Include(u => u.Role)
                     .Include(u => u.Counter)
                     .Where(u => u.RoleId == roleId && u.CounterId == null)
                     .ToListAsync();

        public async Task<List<User>> GetUsersWithRoleCashier(int roleId)
            => await _dbSet
                     .Include(u => u.Role)
                     .Include(u => u.Counter)
                     .Where(u => u.RoleId == roleId && u.CounterId == null)
                     .ToListAsync();
    }
}
