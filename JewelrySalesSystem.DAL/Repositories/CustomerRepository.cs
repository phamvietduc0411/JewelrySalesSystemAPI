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
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(JewelryDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Customer?> GetCustomerByNameAsync(string customerName)
            => await _dbSet
            .Where(c => c.FullName.Equals(customerName))
            .FirstOrDefaultAsync();

        public async Task<Customer?> GetCustomerByPhoneAsync(string phoneNumber)
        => await _dbSet
            .Where(c => c.PhoneNumber.Equals(phoneNumber))
            .FirstOrDefaultAsync();

        public async Task<PaginatedList<Customer>> PaginationAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page, int pageSize)
        {
            IQueryable<Customer> customersQuery = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                customersQuery = customersQuery.Where(c =>
                    c.PhoneNumber.Contains(searchTerm));
            }

            if (sortOrder?.ToLower() == "asc")
            {
                customersQuery = customersQuery.OrderBy(GetSortProperty(sortColumn));
            }
            else
            {
                customersQuery = customersQuery.OrderByDescending(GetSortProperty(sortColumn));
            }

            var customers = await PaginatedList<Customer>.CreateAsync(customersQuery, page, pageSize);

            return customers;
        }
        private static Expression<Func<Customer, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "name" => customer => customer.FullName,
            //"dob" => customer => customer.DoB,
            _ => customer => customer.CustomerId
        };
    }
}

