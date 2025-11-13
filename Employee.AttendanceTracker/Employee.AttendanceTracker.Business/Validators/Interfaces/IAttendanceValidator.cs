using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Validators.Interfaces
{
    public interface IAttendanceValidator
    {
        Task<List<string>> ValidateCreateAttendanceAsync(CreateAttendanceRequestDto requestDto);
        Task<List<string>> ValidateGetAttendanceSummaryByDepartmentAsync(Guid dept, DateTime date);
    }
}
