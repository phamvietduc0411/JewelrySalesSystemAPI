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
    public class CounterRepository : GenericRepository<Counter>, ICounterRepository
    {
        public CounterRepository(JewelryDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<PaginatedList<Counter>> PaginationAsync(string? searchTerm, string? sortColumn, string? sortOrder, bool isActive, int page, int pageSize)
        {
            IQueryable<Counter> countersQuery = _dbSet
                .Include(u => u.CounterType)
                .Include(u => u.User);

            if (isActive) countersQuery = countersQuery.Where(u => u.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                countersQuery = countersQuery.Where(c =>
                    c.CounterName.Contains(searchTerm));
            }

            if (sortOrder?.ToLower() == "asc")
            {
                countersQuery = countersQuery.OrderBy(GetSortProperty(sortColumn));
            }
            else
            {
                countersQuery = countersQuery.OrderByDescending(GetSortProperty(sortColumn));
            }

            var counters = await PaginatedList<Counter>.CreateAsync(countersQuery, page, pageSize);

            return counters;
        }

        private static Expression<Func<Counter, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "name" => counter => counter.CounterName,
            //"dob" => user => user.DoB,
            _ => counter => counter.CounterId
        };

        public async Task<Counter?> GetByIdWithIncludeAsync(int id) => await _dbSet.Include(c => c.User).FirstOrDefaultAsync(c => c.CounterId == id);
    }
}
