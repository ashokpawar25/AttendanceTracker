using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Implementations;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Employee.AttendanceTracker.Business.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeValidator _employeeValidator;
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, IEmployeeValidator employeeValidator,
            IAttendanceService attendanceService, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidator;
            _attendanceService = attendanceService;
            this._logger = logger;
        }

        public async Task<ResponseDto> CreateEmployeeAsync(CreateEmployeeRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _employeeValidator.ValidateCreateEmployeeAsync(requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var employee = new Data.Models.Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    Name = requestDto.Name,
                    Email = requestDto.Email,
                    Password = requestDto.Password,
                    JoinDate = requestDto.JoinDate,
                    DepartmentId = requestDto.DepartmentId,
                    RoleId = requestDto.RoleId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                var createdEmployee = await _employeeRepository.CreateEmployeeAsync(employee);

                response.IsSuccess = true;
                response.Message = "Employee created successfully.";
                _logger.LogInformation("Employee created successfully.");
                response.Result = createdEmployee.EmployeeId;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetAllEmployeesAsync()
        {
            var response = new ResponseDto();
            try
            {
                List<Data.Models.Employee> employees = await _employeeRepository.GetAllEmployeesAsync();
                response.IsSuccess = true;
                response.Message = "Employees retrieved successfully.";
                _logger.LogInformation("Employees retrieved successfully.");
                response.Result = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetEmployeeByIdAsync(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _employeeValidator.ValidateGetEmployeeByIdAsync(id);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                Data.Models.Employee employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                response.IsSuccess = true;
                response.Message = "Employee retrieved successfully.";
                _logger.LogInformation("Employee retreved successfully.");
                response.Result = employee;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetEmployeeByEmailAsync(string email)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _employeeValidator.ValidateGetEmployeeByEmailAsync(email);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                Data.Models.Employee employee = await _employeeRepository.GetEmployeeByEmailAsync(email);
                response.IsSuccess = true;
                response.Message = "Employee retrieved successfully.";
                _logger.LogInformation("Employee retreved successfully.");
                response.Result = employee;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _employeeValidator.ValidateUpdateEmployeeAsync(id, requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

                existingEmployee.Name = requestDto.Name;
                existingEmployee.Email = requestDto.Email;
                existingEmployee.DepartmentId = requestDto.DepartmentId;
                existingEmployee.RoleId = requestDto.RoleId;
                existingEmployee.UpdatedDate = DateTime.Now;

                await _employeeRepository.UpdateEmployeeAsync(existingEmployee);

                response.IsSuccess = true;
                response.Message = "Employee updated successfully.";
                _logger.LogInformation("Employee updated successfully.");
                response.Result = existingEmployee;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> DeleteEmployeeAsync(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _employeeValidator.ValidateDeleteEmployeeAsync(id);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
                await _employeeRepository.DeleteEmployeeAsync(existingEmployee);

                response.IsSuccess = true;
                response.Message = "Employee deleted successfully.";
                _logger.LogInformation("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetEmployeeAttendanceHistoryAsync(Guid id, DateTime? fromDate, DateTime? toDate)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _employeeValidator.ValidateGetEmployeeAttendanceHistoryAsync(id);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                response = await _attendanceService.GetEmployeeAttendanceHistoryAsync(id, fromDate, toDate);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<byte[]> GenerateEmployeeAttendanceHistoryCsvAsync(Guid id, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var errors = await _employeeValidator.ValidateGetEmployeeAttendanceHistoryAsync(id);
                if (errors.Any())
                {
                    return Array.Empty<byte>();
                }

                var attendancesResult = await _attendanceService.GetEmployeeAttendanceHistoryAsync(id, fromDate, toDate);
                var attendances = attendancesResult.Result as List<Attendance>;

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
