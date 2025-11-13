using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Validators.Interfaces
{
    public interface IDepartmentValidator
    {
        Task<List<string>> ValidateCreateDepartmentAsync(CreateDepartmentRequestDto requestDto);
        Task<List<string>> ValidateDeleteDepartmentAsync(Guid id);
        Task<List<string>> ValidateGetDepartmentByIdAsync(Guid id);
        Task<List<string>> ValidateUpdateDepartmentAsync(Guid id, UpdateDepartmentRequestDto requestDto);
    }
}
