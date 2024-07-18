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
    public class WarrantyRepository : GenericRepository<Warranty>, IWarrantyRepository
    {
        public WarrantyRepository(
            JewelryDbContext context,
            ILogger logger) : base(context, logger)
        {
        }

        public async Task<PaginatedList<Warranty>> PaginationAsync(
            string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , int page
            , int pageSize)
        {
            IQueryable<Warranty> warrantiesQuery = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                warrantiesQuery = warrantiesQuery.Where(c =>
                    c.Description.Contains(searchTerm) ||
                    c.StartDate.Equals(searchTerm) ||
                    c.EndDate.Equals(searchTerm));
            }

            if (sortOrder?.ToLower() == "desc")
            {
                warrantiesQuery = warrantiesQuery.OrderByDescending(GetSortProperty(sortColumn));
            }
            else
            {
                warrantiesQuery = warrantiesQuery.OrderBy(GetSortProperty(sortColumn));
            }

            var warranties = await PaginatedList<Warranty>.CreateAsync(warrantiesQuery, page, pageSize);

            return warranties;
        }

        private static Expression<Func<Warranty, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "date" => warranty => warranty.StartDate,
            //"dob" => warranty => warranty.DoB,
            _ => warranty => warranty.WarrantyId
        };

        /*Update Warranty*/
        public async void UpdateWarranty(Warranty warranty)
        {

            var checkExistWarrantyTask = _dbSet.FindAsync(warranty.WarrantyId);
            //var checkExistWarrantyTask = _dbSet.Find(warranty.WarrantyId);
            var checkExistWarranty = await checkExistWarrantyTask;
            if (checkExistWarranty == null)
            {
                throw new Exception($"Warranty with ID {warranty.WarrantyId} not found.");
            }
            //Update data
            checkExistWarranty.Description = warranty.Description;
            checkExistWarranty.StartDate = warranty.StartDate;
            checkExistWarranty.EndDate = warranty.EndDate;
            

        }
        //change here
        public async Task<Warranty> CreateWarranty(Warranty warranty)
        {
            _context.Warranties.Add(warranty);
            await _context.SaveChangesAsync();
            return warranty;
        }
    }
}
