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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDepartmentValidator _departmentValidator;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IDepartmentRepository departmentRepository, IDepartmentValidator departmentValidator,ILogger<DepartmentService> logger)
        {
            _departmentRepository = departmentRepository;
            _departmentValidator = departmentValidator;
            this._logger = logger;
        }

        public async Task<ResponseDto> CreateDepartmentAsync(CreateDepartmentRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _departmentValidator.ValidateCreateDepartmentAsync(requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var department = new Department
                {
                    DepartmentId = Guid.NewGuid(),
                    Name = requestDto.DepartmentName,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                var createdDepartment = await _departmentRepository.CreateDepartmentAsync(department);

                response.IsSuccess = true;
                response.Message = "Department created successfully.";
                _logger.LogInformation("Department created successfully.");
                response.Result = createdDepartment.DepartmentId;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetAllDepartmentsAsync()
        {
            var response = new ResponseDto();
            try
            {
                List<Department> departments = await _departmentRepository.GetAllDepartmentsAsync();
                response.IsSuccess = true;
                response.Message = "Departments retrieved successfully.";
                _logger.LogInformation("Departments retrieved successfully.");
                response.Result = departments;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetDepartmentByIdAsync(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _departmentValidator.ValidateGetDepartmentByIdAsync(id);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                Department department = await _departmentRepository.GetDepartmentByIdAsync(id);
                response.IsSuccess = true;
                response.Message = "Department retrieved successfully.";
                _logger.LogInformation("Department retrieved successfully.");
                response.Result = department;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _departmentValidator.ValidateUpdateDepartmentAsync(id, requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
                existingDepartment.Name = requestDto.DepartmentName;
                existingDepartment.UpdatedDate = DateTime.Now;

                await _departmentRepository.UpdateDepartmentAsync(existingDepartment);

                response.IsSuccess = true;
                response.Message = "Department updated successfully.";
                _logger.LogInformation("Department updated successfully.");
                response.Result = existingDepartment;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> DeleteDepartmentAsync(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var errors = await _departmentValidator.ValidateDeleteDepartmentAsync(id);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(id);
                await _departmentRepository.DeleteDepartmentAsync(existingDepartment);

                response.IsSuccess = true;
                response.Message = "Department deleted successfully.";
                _logger.LogInformation("Department deleted successfully.");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }
    }
}
