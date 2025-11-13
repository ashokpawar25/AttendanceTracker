using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Data.DatabaseContexts;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using EmployeeModel = Employee.AttendanceTracker.Data.Models.Employee;

namespace Employee.AttendanceTracker.Data.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public EmployeeRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeModel> CreateEmployeeAsync(EmployeeModel employee)
        {
            var createdEmployee = await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
            return createdEmployee.Entity;
        }

        public async Task<List<EmployeeModel>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(Guid id)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<EmployeeModel> GetEmployeeByEmailAsync(string email)
        {
            return await _dbContext.Employees.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task UpdateEmployeeAsync(EmployeeModel existingEmployee)
        {
            _dbContext.Employees.Update(existingEmployee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(EmployeeModel existingEmployee)
        {
            _dbContext.Employees.Remove(existingEmployee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
