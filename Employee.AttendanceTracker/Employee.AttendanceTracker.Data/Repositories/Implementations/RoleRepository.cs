using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Data.DatabaseContexts;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Employee.AttendanceTracker.Data.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public RoleRepository(AttendanceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Role> CreateRoleAsync(Role role)
        {
            var createdRole = await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
            return createdRole.Entity;
        }

        public async Task DeleteRoleAsync(Role role)
        {
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task UpdateRoleAsync(Role existingRole)
        {
            _dbContext.Roles.Update(existingRole);
            await _dbContext.SaveChangesAsync();
        }
    }
}
