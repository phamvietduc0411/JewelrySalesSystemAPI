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
    public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(
            JewelryDbContext context,
            ILogger logger) : base(context, logger)
        {
        }

        public async Task<PaginatedList<Material>> PaginationAsync(
            string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize)
        {
            IQueryable<Material> materialsQuery = _dbSet
                .Include(m => m.MaterialPrices
                .OrderByDescending(m => m.EffDate)
                .Take(1));

            if (isActive) materialsQuery = materialsQuery.Where(m => m.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                materialsQuery = materialsQuery.Where(c =>
                    c.MaterialName.Contains(searchTerm));
            }

            if (sortOrder?.ToLower() == "asc")
            {
                materialsQuery = materialsQuery.OrderBy(GetSortProperty(sortColumn));
            }
            else
            {
                materialsQuery = materialsQuery.OrderByDescending(GetSortProperty(sortColumn));
            }

            var materials = await PaginatedList<Material>.CreateAsync(materialsQuery, page, pageSize);

            return materials;
        }

        private static Expression<Func<Material, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "name" => material => material.MaterialName,
            //"dob" => material => material.DoB,
            _ => material => material.MaterialId
        };

        public async Task<Material?> GetByIdWithIncludeAsync(int id)
        {
            var result = await _dbSet
                                .Include(m => m.MaterialPrices.OrderByDescending(m => m.EffDate).Take(1))
                                .FirstOrDefaultAsync(x => x.MaterialId == id);
            if (result == null) return null;
            return result;
        }

        public async Task<List<Material>> GetAllGoldMaterials() => await _dbSet
                                .Include(m => m.MaterialPrices.OrderByDescending(m => m.EffDate).Take(1))
                                .Where(m => String.Equals(m.MaterialName, "Gold", StringComparison.OrdinalIgnoreCase))
                                .ToListAsync();

        public async Task<Material?> GetByNameWithIncludeAsync(string name)
        {
            var result = await _dbSet
                                .Include(m => m.MaterialPrices.OrderByDescending(m => m.EffDate).Take(1))
                                .FirstOrDefaultAsync(x => x.MaterialName.Equals(name));
            if (result == null) return null;
            return result;
        }

        //changes here
        public async Task<Material> CheckDuplicate(string materialName) => await _dbSet.FirstOrDefaultAsync(m => m.MaterialName == materialName);
    }
}
