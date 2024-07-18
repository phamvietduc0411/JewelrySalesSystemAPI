using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IColorRepository : IGenericRepository<Color>
    {
        Task<List<Color>> GetAllAsync();
    }
}
