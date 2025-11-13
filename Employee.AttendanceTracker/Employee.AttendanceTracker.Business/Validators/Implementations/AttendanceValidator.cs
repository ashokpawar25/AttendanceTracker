using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.DatabaseContexts;
using Employee.AttendanceTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee.AttendanceTracker.Business.Validators.Implementations
{
    public class AttendanceValidator : IAttendanceValidator
    {
        private readonly AttendanceDbContext _dbContext;

        public AttendanceValidator(AttendanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> ValidateCreateAttendanceAsync(CreateAttendanceRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if (requestDto.EmployeeId == Guid.Empty)
            {
                errors.Add("EmployeeId is required.");
            }
            else if (!await _dbContext.Employees.AnyAsync(e => e.EmployeeId == requestDto.EmployeeId))
            {
                errors.Add("Employee not found.");
            }

            if (requestDto.AttendanceDate == default)
            {
                errors.Add("AttendanceDate is required.");
            }

            if (!Enum.IsDefined(typeof(AttendanceStatus), requestDto.AttendanceStatus))
            {
                errors.Add("Invalid AttendanceStatus.");
            }

            if (await _dbContext.Attendances.AnyAsync(a =>
                a.EmployeeId == requestDto.EmployeeId &&
                a.AttendanceDate.Date == requestDto.AttendanceDate.Date))
            {
                errors.Add($"Attendance already marked for this employee for date {requestDto.AttendanceDate}.");
            }

            return errors;
        }

        public async Task<List<string>> ValidateGetAttendanceSummaryByDepartmentAsync(Guid deptId, DateTime date)
        {
            List<string> errors = new List<string>();

            if (deptId == Guid.Empty)
            {
                errors.Add("Department Id is required.");
            }
            else if (!await _dbContext.Departments.AnyAsync(d => d.DepartmentId == deptId))
            {
                errors.Add("Department not found.");
            }

            if (date == default)
            {
                errors.Add("Date is required.");
            }

            return errors;
        }
    }
}
