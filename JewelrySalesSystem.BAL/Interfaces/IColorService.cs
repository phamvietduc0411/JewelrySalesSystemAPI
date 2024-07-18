using JewelrySalesSystem.BAL.Models.Colors;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IColorService
    {
        Task<List<GetColorResponse>> GetAllAsync();

        Task<GetColorResponse?> GetByIdAsync(int id);
    }
}
