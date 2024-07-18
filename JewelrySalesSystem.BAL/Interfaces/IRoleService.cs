using JewelrySalesSystem.BAL.Models.Roles;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IRoleService
    {
        Task<CreateRoleRequest> AddRoleAsync(CreateRoleRequest role);

        Task<List<GetRoleResponse>> GetAllAsync();
    }
}
