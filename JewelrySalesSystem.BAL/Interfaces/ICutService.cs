using JewelrySalesSystem.BAL.Models.Cuts;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface ICutService
    {
        Task<List<GetCutResponse>> GetAllAsync();
    }
}
