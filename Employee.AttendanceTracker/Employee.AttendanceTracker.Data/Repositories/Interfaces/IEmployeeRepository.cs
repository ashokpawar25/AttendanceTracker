using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.AttendanceTracker.Data.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Data.Models.Employee> CreateEmployeeAsync(Models.Employee employee);
        Task DeleteEmployeeAsync(Models.Employee existingEmployee);
        Task<List<Models.Employee>> GetAllEmployeesAsync();
        Task<Models.Employee> GetEmployeeByEmailAsync(string email);
        Task<Models.Employee> GetEmployeeByIdAsync(Guid id);
        Task UpdateEmployeeAsync(Models.Employee existingEmployee);
    }
}
