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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleValidator _roleValidator;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository roleRepository, IRoleValidator roleValidator, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _roleValidator = roleValidator;
            this._logger = logger;
        }

        public async Task<ResponseDto> CreateRoleAsync(CreateRoleRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _roleValidator.ValidateCreateRoleAsync(requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var role = new Role
                {
                    RoleId = Guid.NewGuid(),
                    Name = requestDto.RoleName,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                Role createdRole = await _roleRepository.CreateRoleAsync(role);
                response.IsSuccess = true;
                response.Message = "Role created successfully.";
                response.Result = createdRole.RoleId;
                _logger.LogInformation("Role created successfully.");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}.";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> DeleteRoleAsync(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _roleValidator.ValidateDeleteRoleAsync(id);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }
                Role existingRole = await _roleRepository.GetRoleByIdAsync(id);
                await _roleRepository.DeleteRoleAsync(existingRole);
                response.IsSuccess = true;
                response.Message = "Role deleted successfully.";
                _logger.LogInformation("Role deleted successfully.");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error occurred {ex.Message}.";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetAllRolesAsync()
        {
            var response = new ResponseDto();
            try
            {
                var roles = await _roleRepository.GetAllRolesAsync();

                response.IsSuccess = true;
                response.Message = "Roles retrieved successfully.";
                _logger.LogInformation("Roles retrieved successfully.");
                response.Result = roles;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> GetRoleByIdAsync(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _roleValidator.ValidateGetRoleByIdAsync(id);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                Role role = await _roleRepository.GetRoleByIdAsync(id);
                response.IsSuccess = true;
                response.Message = "Role retrieved successfully.";
                _logger.LogInformation("Role retreved successfully.");
                response.Result = role;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error occurred {ex.Message}.";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseDto> UpdateRoleAsync(Guid id, UpdateRoleRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _roleValidator.ValidateUpdateRoleAsync(id, requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                Role existingRole = await _roleRepository.GetRoleByIdAsync(id);
                existingRole.Name = requestDto.RoleName;
                existingRole.UpdatedDate = DateTime.Now;

                await _roleRepository.UpdateRoleAsync(existingRole);

                response.IsSuccess = true;
                response.Message = "Role updated successfully.";
                _logger.LogInformation("Role updated successfully.");
                response.Result = existingRole;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error occurred {ex.Message}.";
                _logger.LogError($"Error occurred: {ex.Message}");
            }

            return response;
        }
    }
}
