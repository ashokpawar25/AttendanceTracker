using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Validators.Interfaces
{
    public interface IEmployeeValidator
    {
        Task<List<string>> ValidateCreateEmployeeAsync(CreateEmployeeRequestDto requestDto);
        Task<List<string>> ValidateDeleteEmployeeAsync(Guid id);
        Task<List<string>> ValidateGetEmployeeAttendanceHistoryAsync(Guid id);
        Task<List<string>> ValidateGetEmployeeByEmailAsync(string email);
        Task<List<string>> ValidateGetEmployeeByIdAsync(Guid id);
        Task<List<string>> ValidateUpdateEmployeeAsync(Guid id, UpdateEmployeeRequestDto requestDto);
    }
}
