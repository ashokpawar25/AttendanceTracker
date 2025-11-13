using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace Employee.AttendanceTracker.Business.Validators.Implementations
{
    public class RoleValidator : IRoleValidator
    {
        private readonly AttendanceDbContext _dbContext;
        public RoleValidator(AttendanceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<List<string>> ValidateCreateRoleAsync(CreateRoleRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if(string.IsNullOrWhiteSpace(requestDto.RoleName))
            {
                errors.Add("Role name is required.");
            }

            if(await _dbContext.Roles.AnyAsync(r => r.Name == requestDto.RoleName))
            {
                errors.Add("Duplicate role found.");
            }
            return errors;
        }

        public async Task<List<string>> ValidateDeleteRoleAsync(Guid id)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Roles.AnyAsync(r => r.RoleId == id))
            {
                errors.Add("Role not found.");
            }
            return errors;
        }

        public async Task<List<string>> ValidateGetRoleByIdAsync(Guid id)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Roles.AnyAsync(r => r.RoleId == id))
            {
                errors.Add("Role not found.");
            }
            return errors;
        }

        public async Task<List<string>> ValidateUpdateRoleAsync(Guid id, UpdateRoleRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Roles.AnyAsync(r => r.RoleId == id))
            {
                errors.Add("Role not found.");
            }

            if (string.IsNullOrWhiteSpace(requestDto.RoleName))
            {
                errors.Add("Role name is required.");
            }

            if (await _dbContext.Roles.AnyAsync(r => r.Name == requestDto.RoleName))
            {
                errors.Add("Duplicate role found.");
            }
            return errors;
        }
    }
}
