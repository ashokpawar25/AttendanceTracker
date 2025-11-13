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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public DepartmentRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Department> CreateDepartmentAsync(Department department)
        {
            var createdDepartment = await _dbContext.Departments.AddAsync(department);
            await _dbContext.SaveChangesAsync();
            return createdDepartment.Entity;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _dbContext.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(Guid id)
        {
            return await _dbContext.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
        }

        public async Task UpdateDepartmentAsync(Department existingDepartment)
        {
            _dbContext.Departments.Update(existingDepartment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(Department existingDepartment)
        {
            _dbContext.Departments.Remove(existingDepartment);
            await _dbContext.SaveChangesAsync();
        }
    }

}
