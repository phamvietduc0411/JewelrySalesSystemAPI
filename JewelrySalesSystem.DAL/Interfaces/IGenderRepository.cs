using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IGenderRepository : IGenericRepository<Gender>
    {
        Task<List<Gender>> GetAllAsync();

        Task<Gender> AddGender(Gender gender);
    }
}
