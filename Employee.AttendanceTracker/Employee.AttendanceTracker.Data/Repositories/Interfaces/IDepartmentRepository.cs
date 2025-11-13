using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Data.Models;

namespace Employee.AttendanceTracker.Data.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Department> CreateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(Department existingDepartment);
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(Guid id);
        Task UpdateDepartmentAsync(Department existingDepartment);
    }
}
