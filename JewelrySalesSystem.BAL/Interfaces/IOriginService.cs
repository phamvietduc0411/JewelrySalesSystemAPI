using JewelrySalesSystem.BAL.Models.Origins;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IOriginService
    {
        Task<List<GetOriginResponse>> GetAllAsync();
    }
}
