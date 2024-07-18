using JewelrySalesSystem.BAL.Models.CounterTypes;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface ICounterTypeService
    {
        Task<List<GetCounterTypeResponse>> GetAllAsync();
    }
}
