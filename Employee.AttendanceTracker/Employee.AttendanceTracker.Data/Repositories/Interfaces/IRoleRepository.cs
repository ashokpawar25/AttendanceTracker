using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Data.Models;

namespace Employee.AttendanceTracker.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateRoleAsync(Role role);
        Task DeleteRoleAsync(Role role);
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(Guid id);
        Task UpdateRoleAsync(Role existingRole);
    }
}
