using JewelrySalesSystem.BAL.Models.Shapes;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IShapeService
    {
        Task<List<GetShapeResponse>> GetAllAsync();
    }
}
