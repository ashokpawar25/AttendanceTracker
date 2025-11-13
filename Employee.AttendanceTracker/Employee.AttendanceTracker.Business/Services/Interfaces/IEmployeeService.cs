using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ResponseDto> CreateEmployeeAsync(CreateEmployeeRequestDto requestDto);
        Task<ResponseDto> DeleteEmployeeAsync(Guid id);
        Task<byte[]> GenerateEmployeeAttendanceHistoryCsvAsync(Guid id, DateTime? fromDate, DateTime? toDate);
        Task<ResponseDto> GetAllEmployeesAsync();
        Task<ResponseDto> GetEmployeeAttendanceHistoryAsync(Guid id, DateTime? fromDate, DateTime? toDate);
        Task<ResponseDto> GetEmployeeByEmailAsync(string email);
        Task<ResponseDto> GetEmployeeByIdAsync(Guid id);
        Task<ResponseDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequestDto requestDto);
    }
}
