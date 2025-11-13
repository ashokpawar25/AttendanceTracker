using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Data.Models;

namespace Employee.AttendanceTracker.Business.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<ResponseDto> CreateAttendanceAsync(CreateAttendanceRequestDto requestDto);
        Task<byte[]> GenerateAttendanceSummaryCsvAsync(Guid departmentId, DateTime date);
        Task<ResponseDto> GetAttendanceSummaryByDepartmentAsync(Guid departmentId, DateTime date);
        Task<ResponseDto> GetEmployeeAttendanceHistoryAsync(Guid id, DateTime? fromDate, DateTime? toDate);
    }
}
