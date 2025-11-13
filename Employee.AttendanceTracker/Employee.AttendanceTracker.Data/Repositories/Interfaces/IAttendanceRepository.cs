using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Data.Models;

namespace Employee.AttendanceTracker.Data.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<Attendance> CreateAttendanceAsync(Attendance attendance);
        Task<List<Attendance>> GetAttendanceSummaryByDepartmentAsync(Guid dept, DateTime date);
        Task<List<Attendance>> GetEmployeeAttendanceHistoryAsync(Guid id, DateTime? fromDate, DateTime? toDate);
    }
}
