using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Data.DatabaseContexts;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Employee.AttendanceTracker.Data.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AttendanceDbContext _dbContext;

        public AttendanceRepository(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendance)
        {
            var createdAttendance = await _dbContext.Attendances.AddAsync(attendance);
            await _dbContext.SaveChangesAsync();
            return createdAttendance.Entity;
        }

        public async Task<List<Attendance>> GetAttendanceSummaryByDepartmentAsync(Guid deptId, DateTime date)
        {
            var attendances = await _dbContext.Attendances
                .Where(a => a.AttendanceDate.Date == date.Date && a.Employee.Department.DepartmentId == deptId)
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .Include(a => a.Employee)
                .ThenInclude(e => e.Role)
                .OrderByDescending(a => a.AttendanceDate)
                .ToListAsync();

            return attendances;
        }

        public async Task<List<Attendance>> GetEmployeeAttendanceHistoryAsync(Guid id, DateTime? fromDate, DateTime? toDate)
        {
            var history = _dbContext.Attendances
                .Where(a => a.EmployeeId == id)
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .Include(a => a.Employee)
                .ThenInclude(e => e.Role)
                .OrderByDescending(a => a.AttendanceDate)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                var from = fromDate.Value.Date;
                history = history.Where(h => h.AttendanceDate.Date >= from);
            }

            if (toDate.HasValue)
            {
                var to = toDate.Value.Date;
                history = history.Where(h => h.AttendanceDate.Date <= to);
            }

            return await history.ToListAsync();
        }
    }
}
