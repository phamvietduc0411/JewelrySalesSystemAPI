using JewelrySalesSystem.BAL.Models.Carats;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface ICaratService
    {
        Task<List<GetCaratWeightResponse>> GetAllAsync();
    }
}
