using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Employee.AttendanceTracker.Business.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IAttendanceValidator _attendanceValidator;
        private readonly ILogger<AttendanceService> _logger;

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IAttendanceValidator attendanceValidator,
            ILogger<AttendanceService> logger)
        {
            _attendanceRepository = attendanceRepository;
            _attendanceValidator = attendanceValidator;
            this._logger = logger;
        }

        public async Task<ResponseDto> CreateAttendanceAsync(CreateAttendanceRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _attendanceValidator.ValidateCreateAttendanceAsync(requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var attendance = new Attendance
                {
                    AttendanceId = Guid.NewGuid(),
                    EmployeeId = requestDto.EmployeeId,
                    AttendanceDate = requestDto.AttendanceDate,
                    AttendanceStatus = requestDto.AttendanceStatus,
                    CreatedDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                Attendance createdAttendance = await _attendanceRepository.CreateAttendanceAsync(attendance);

                response.IsSuccess = true;
                response.Message = "Attendance created successfully.";
                _logger.LogInformation("Attendance created successfully.");
                response.Result = createdAttendance;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetAttendanceSummaryByDepartmentAsync(Guid departmentId, DateTime date)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _attendanceValidator.ValidateGetAttendanceSummaryByDepartmentAsync(departmentId, date);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var summary = await _attendanceRepository.GetAttendanceSummaryByDepartmentAsync(departmentId, date);

                response.IsSuccess = true;
                response.Message = "Attendance summary retrieved successfully.";
                _logger.LogInformation("Attendance summary retrieved successfully.");
                response.Result = summary;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetEmployeeAttendanceHistoryAsync(Guid id, DateTime? fromDate, DateTime? toDate)
        {
            var response = new ResponseDto();
            try
            {
                var history = await _attendanceRepository.GetEmployeeAttendanceHistoryAsync(id, fromDate, toDate);

                response.IsSuccess = true;
                response.Message = "Employee attendance history retrieved successfully.";
                _logger.LogInformation("Employee attendance history retrieved successfully.");
                response.Result = history;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<byte[]> GenerateAttendanceSummaryCsvAsync(Guid departmentId, DateTime date)
        {
            try
            {
                var attendances = await _attendanceRepository.GetAttendanceSummaryByDepartmentAsync(departmentId, date);

                if (attendances == null || !attendances.Any())
                    return Array.Empty<byte>();

                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("EmployeeId,EmployeeName,DepartmentName,Date,Status");

                foreach (var attendance in attendances)
                {
                    csvBuilder.AppendLine($"{attendance.Employee.EmployeeId}," +
                                          $"{attendance.Employee.Name}," +
                                          $"{attendance.Employee.Department.Name}," +
                                          $"{attendance.AttendanceDate:yyyy-MM-dd}," +
                                          $"{attendance.AttendanceStatus}");
                }

                return Encoding.UTF8.GetBytes(csvBuilder.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating CSV: {ex.Message}");
                throw;
            }
        }
    }
}
