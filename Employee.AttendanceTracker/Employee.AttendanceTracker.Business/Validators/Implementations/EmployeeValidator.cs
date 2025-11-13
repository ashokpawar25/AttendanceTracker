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
    public class EmployeeValidator : IEmployeeValidator
    {
        private readonly AttendanceDbContext _dbContext;

        public EmployeeValidator(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> ValidateCreateEmployeeAsync(CreateEmployeeRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(requestDto.Name))
                errors.Add("Employee name is required.");

            if (string.IsNullOrWhiteSpace(requestDto.Email))
                errors.Add("Employee email is required.");

            if (!await _dbContext.Departments.AnyAsync(d => d.DepartmentId == requestDto.DepartmentId))
                errors.Add("Department not found.");

            if (!await _dbContext.Roles.AnyAsync(r => r.RoleId == requestDto.RoleId))
                errors.Add("Role not found.");

            if (await _dbContext.Employees.AnyAsync(e => e.Email == requestDto.Email))
                errors.Add("Duplicate email found.");

            return errors;
        }

        public async Task<List<string>> ValidateGetEmployeeByIdAsync(Guid id)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Employees.AnyAsync(e => e.EmployeeId == id))
                errors.Add("Employee not found.");

            return errors;
        }

        public async Task<List<string>> ValidateGetEmployeeByEmailAsync(string email)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Employees.AnyAsync(e => e.Email == email))
                errors.Add("Employee not found.");

            return errors;
        }

        public async Task<List<string>> ValidateUpdateEmployeeAsync(Guid id, UpdateEmployeeRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Employees.AnyAsync(e => e.EmployeeId == id))
                errors.Add("Employee not found.");

            if (string.IsNullOrWhiteSpace(requestDto.Name))
                errors.Add("Employee name is required.");

            if (string.IsNullOrWhiteSpace(requestDto.Email))
                errors.Add("Employee email is required.");

            if (!await _dbContext.Departments.AnyAsync(d => d.DepartmentId == requestDto.DepartmentId))
                errors.Add("Department not found.");

            if (!await _dbContext.Roles.AnyAsync(r => r.RoleId == requestDto.RoleId))
                errors.Add("Role not found.");

            if (await _dbContext.Employees.AnyAsync(e => e.Email == requestDto.Email && e.EmployeeId != id))
                errors.Add("Duplicate email found.");

            return errors;
        }

        public async Task<List<string>> ValidateDeleteEmployeeAsync(Guid id)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Employees.AnyAsync(e => e.EmployeeId == id))
                errors.Add("Employee not found.");

            return errors;
        }

        public async Task<List<string>> ValidateGetEmployeeAttendanceHistoryAsync(Guid id)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Employees.AnyAsync(e => e.EmployeeId == id))
                errors.Add("Employee not found.");

            return errors;
        }
    }
}
