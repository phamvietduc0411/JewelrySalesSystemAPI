using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class GenderRepository : GenericRepository<Gender>, IGenderRepository
    {
        public GenderRepository(
            JewelryDbContext context
            , ILogger logger) : base(context, logger)
        {
        }

        public async Task<Gender> AddGender(Gender gender)
        {
            _context.Genders.Add(gender);

            await _context.SaveChangesAsync();

            return gender;
        }

        public async Task<List<Gender>> GetAllAsync() => await _dbSet.ToListAsync();
    }
}
