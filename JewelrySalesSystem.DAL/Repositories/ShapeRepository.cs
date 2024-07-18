using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.Extensions.Logging;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class ShapeRepository : GenericRepository<Shape>, IShapeRepository
    {
        public ShapeRepository(JewelryDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
