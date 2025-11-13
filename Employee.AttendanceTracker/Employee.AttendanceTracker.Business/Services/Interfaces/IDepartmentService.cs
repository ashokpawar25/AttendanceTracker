using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<ResponseDto> CreateDepartmentAsync(CreateDepartmentRequestDto requestDto);
        Task<ResponseDto> DeleteDepartmentAsync(Guid id);
        Task<ResponseDto> GetAllDepartmentsAsync();
        Task<ResponseDto> GetDepartmentByIdAsync(Guid id);
        Task<ResponseDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequestDto requestDto);
    }
}
