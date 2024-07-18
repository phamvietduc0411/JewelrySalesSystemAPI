using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelrySalesSystem.BAL.Models.Roles
{
    public class GetRoleResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
