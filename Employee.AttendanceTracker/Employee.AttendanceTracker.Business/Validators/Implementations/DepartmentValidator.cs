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
    public class DepartmentValidator : IDepartmentValidator
    {
        private readonly AttendanceDbContext _dbContext;

        public DepartmentValidator(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> ValidateCreateDepartmentAsync(CreateDepartmentRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(requestDto.DepartmentName))
            {
                errors.Add("Department name is required.");
            }

            if (await _dbContext.Departments.AnyAsync(d => d.Name == requestDto.DepartmentName))
            {
                errors.Add("Duplicate department found.");
            }

            return errors;
        }

        public async Task<List<string>> ValidateDeleteDepartmentAsync(Guid id)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Departments.AnyAsync(d => d.DepartmentId == id))
            {
                errors.Add("Department not found.");
            }

            return errors;
        }

        public async Task<List<string>> ValidateGetDepartmentByIdAsync(Guid id)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Departments.AnyAsync(d => d.DepartmentId == id))
            {
                errors.Add("Department not found.");
            }

            return errors;
        }

        public async Task<List<string>> ValidateUpdateDepartmentAsync(Guid id, UpdateDepartmentRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if (!await _dbContext.Departments.AnyAsync(d => d.DepartmentId == id))
            {
                errors.Add("Department not found.");
            }

            if (string.IsNullOrWhiteSpace(requestDto.DepartmentName))
            {
                errors.Add("Department name is required.");
            }

            if (await _dbContext.Departments.AnyAsync(d => d.Name == requestDto.DepartmentName))
            {
                errors.Add("Duplicate department found.");
            }

            return errors;
        }
    }
}
