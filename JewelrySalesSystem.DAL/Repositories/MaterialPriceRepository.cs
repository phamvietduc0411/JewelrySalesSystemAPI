using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class MaterialPriceRepository : GenericRepository<MaterialPriceList>, IMaterialPriceListRepository
    {
        public MaterialPriceRepository(
           JewelryDbContext context,
           ILogger logger) : base(context, logger)
        {

        }

        public async Task<MaterialPriceList> CreateMaterialPrice(MaterialPriceList materialPrice)
        {
            _context.MaterialPrices.Add(materialPrice);

            await _context.SaveChangesAsync();
            return materialPrice;

        }

        public async Task<MaterialPriceList> GetNewestMaterialPriceByMaterialIdAsync(int materialId)
        {
            var newestMaterialPrice = await _dbSet
                .Where(mpl => mpl.MaterialId == materialId)
                .OrderByDescending(mpl => mpl.EffDate)
                .FirstOrDefaultAsync();

            if (newestMaterialPrice == null)
            {
                // Nếu không tìm thấy bản ghi, hãy trả về một MaterialPriceList mới với giá 0
                return new MaterialPriceList
                {
                    MaterialId = materialId,
                    BuyPrice = 0,
                    SellPrice = 0
                };
            }

            return newestMaterialPrice;

        }
    }
}
