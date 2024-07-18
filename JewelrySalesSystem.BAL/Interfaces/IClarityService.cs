using JewelrySalesSystem.BAL.Models.Clarities;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IClarityService
    {
        Task<List<GetClarityResponse>> GetAllAsync();
    }
}
