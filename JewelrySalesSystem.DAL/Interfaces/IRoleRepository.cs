using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.DAL.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        /*Change here*/
        Task<Role> AddRoleAsync(Role role);

        Task<Role> CheckDuplicate(string name );
    }
}
