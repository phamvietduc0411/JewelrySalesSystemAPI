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
    public class GemRepository : GenericRepository<Gem>, IGemRepository
    {
        public GemRepository(
            JewelryDbContext context,
            ILogger logger) : base(context, logger)
        {
        }

        public async Task<PaginatedList<Gem>> PaginationAsync(
            string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize)
        {
            IQueryable<Gem> gemsQuery = _dbSet
                .Include(g => g.Origin)
                .Include(g => g.Carat)
                .Include(g => g.Cut)
                .Include(g => g.Clarity)
                .Include(g => g.Color)
                .Include(g => g.Shape);

            if (isActive) gemsQuery = gemsQuery.Where(g => g.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                gemsQuery = gemsQuery.Where(c =>
                    c.GemName.Contains(searchTerm));
            }

            if (sortOrder?.ToLower() == "asc")
            {
                gemsQuery = gemsQuery.OrderBy(GetSortProperty(sortColumn));
            }
            else
            {
                gemsQuery = gemsQuery.OrderByDescending(GetSortProperty(sortColumn));
            }

            var gems = await PaginatedList<Gem>.CreateAsync(gemsQuery, page, pageSize);

            return gems;
        }

        private static Expression<Func<Gem, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "name" => gem => gem.GemName,
            //"dob" => gem => gem.DoB,
            _ => gem => gem.GemId
        };

        public async Task<Gem?> GetByIdWithIncludeAsync(int id)
        {
            var result = await _dbSet
                .Include(g => g.Origin)
                .Include(g => g.Carat)
                .Include(g => g.Cut)
                .Include(g => g.Clarity)
                .Include(g => g.Color)
                .Include(g => g.Shape)
                .FirstOrDefaultAsync(g => g.GemId == id);

            if (result == null) return null;
            return result;
        }

        public async Task UpdateGem(Gem gem)
        {
            //Check exist
            var checkExistGemTask = _dbSet.FindAsync(gem.GemId);
            var checkExistGem = await checkExistGemTask;
            if (checkExistGem != null)
            {
                //Update data
                checkExistGem.GemName = gem.GemName;
                checkExistGem.FeaturedImage = gem.FeaturedImage;
                checkExistGem.OriginId = gem.OriginId;
                checkExistGem.CaratId = gem.CaratId;
                checkExistGem.ColorId = gem.ColorId;
                checkExistGem.ClarityId = gem.ClarityId;
                checkExistGem.CutId = gem.CutId;
                checkExistGem.ShapeId = gem.ShapeId;
                //var gemPrice = await _context.GemPrices.FirstOrDefaultAsync(p => p.GemId == gem.GemId);
                //if (gemPrice == null)
                //{
                //    gemPrice = new GemPriceList
                //    {
                //        GemId = gem.GemId,
                //        CaratWeightPrice = gem.GemPrice.CaratWeightPrice,
                //        ColourPrice = gem.GemPrice.ColourPrice,
                //        ClarityPrice = gem.GemPrice.ClarityPrice,
                //        CutPrice = gem.GemPrice.CutPrice,

                //    };
                //    _context.GemPrices.Add(gemPrice);
                //}
                //else
                //{
                //    gemPrice.CaratWeightPrice = gem.GemPrice.CaratWeightPrice;
                //    gemPrice.ColourPrice = gem.GemPrice.ColourPrice;
                //    gemPrice.ClarityPrice = gem.GemPrice.ClarityPrice;
                //    gemPrice.CutPrice = gem.GemPrice.CutPrice;
                //    _context.GemPrices.Update(gemPrice);

                //}
            }
            else
            {
                throw new Exception($"Gem with Id = {gem.GemId} not found!");
            }
            _dbSet.Update(checkExistGem);
        }

        public async Task<Gem?> GetByNameWithIncludeAsync(string name)
        {
            var result = await _dbSet
                .Include(g => g.Origin)
                .Include(g => g.Carat)
                .Include(g => g.Cut)
                .Include(g => g.Clarity)
                .Include(g => g.Color)
                .Include(g => g.Shape)
                .FirstOrDefaultAsync(g => g.GemName.Equals(name));

            if (result == null) return null;
            return result;
        }

        public async Task<float> GetGemPriceAsync(Gem gem)
        {
            var result = await _context.GemPrices
                .OrderByDescending(g => g.EffDate)
                .FirstOrDefaultAsync(g => g.CaratId == gem.CaratId
                                       && g.OriginId == gem.OriginId
                                       && g.CutId == gem.CutId
                                       && g.ClarityId == gem.ClarityId
                                       && g.ColorId == gem.ColorId);

            if (result != null) return result.Price;

            return 1;
        }

        public async Task<float> GetShapePriceRateAsync(int shapeId)
        {
            var result = await _context.Shapes.FindAsync(shapeId);

            if (result != null) return result.PriceRate;

            return 1;
        }

        public async Task<bool> CheckId(int id, string option)
        {
            switch (option)
            {
                case "CaratId":
                    return await _context.Carats.AnyAsync(c => c.CaratId == id);
                    break;

                case "ClarityId":
                    return await _context.Clarities.AnyAsync(c => c.ClarityId == id);
                    break;
                case "ColorId":
                    return await _context.Colors.AnyAsync(c => c.ColorId == id);
                    break;
                case "CutId":
                    return await _context.Cuts.AnyAsync(c => c.CutId == id);
                    break;

                case "OriginId":
                    return await _context.Origins.AnyAsync(c => c.OriginId == id);
                    break;

                case "ShapeId":
                    return await _context.Shapes.AnyAsync(c => c.ShapeId == id);
                    break;

                case "GemId":
                    return await _context.Gems.AnyAsync(g => g.GemId == id);
                default:
                    throw new ArgumentException($"Unknown option: {option}");
            }
        }

        public async Task<float> GetGemPriceAsync(GemPriceList gemPriceList)
        {
            var price = await _context.GemPrices
                .Where(g => g.CaratId == gemPriceList.CaratId
                         && g.ClarityId == gemPriceList.ClarityId
                         && g.ColorId == gemPriceList.ColorId
                         && g.CutId == gemPriceList.CutId
                         && g.OriginId == gemPriceList.OriginId)
                .OrderByDescending(g => g.EffDate)
                .Select(g => g.Price)
                .FirstOrDefaultAsync(); ;

            if (price > 0) return price;

            return 0;
        }

        public async Task<List<GemPriceList>> GetGemPricesAsync() => await _context.GemPrices.OrderByDescending(g => g.Id).ToListAsync();

        public void AddGemPrice(GemPriceList gemPriceList) => _context.GemPrices.Add(gemPriceList);

        public async Task DeleteAsync(int id)
        {
            var gem = await _dbSet.FindAsync(id);

            if (gem != null)
            {
                if (gem.IsActive)
                {
                    gem.IsActive = false;
                } else
                {
                    gem.IsActive = true;
                }               
                _dbSet.Update(gem);
            } else
            {
                throw new Exception($"Gem with {id} does not exist");
            }         
        }
    }
}
