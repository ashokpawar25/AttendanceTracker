using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Employee.AttendanceTracker.Business.Validators.Implementations;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Employee.AttendanceTracker.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthValidator _authValidator;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(IEmployeeService employeeService, IAuthValidator authValidator, ILogger<AuthService> logger, IConfiguration configuration)
        {
            this._employeeService = employeeService;
            this._authValidator = authValidator;
            this._logger = logger;
            this._configuration = configuration;
        }

        public async Task<ResponseDto> Login(LoginRequestDto requestDto)
        {
            var response = new ResponseDto();
            try
            {
                List<string> errors = await _authValidator.ValidateLoginAsync(requestDto);
                if (errors.Any())
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(", ", errors);
                    _logger.LogWarning(string.Join(", ", errors));
                    return response;
                }

                var employeeResponse = await _employeeService.GetEmployeeByEmailAsync(requestDto.Email);
                var employee = employeeResponse.Result as Data.Models.Employee;

                JwtSecurityToken jwtSecurityToken = GenerateToken(employee);
                response = new ResponseDto
                {
                    IsSuccess = true,
                    Message = "User logged in successfully.",
                    Result = new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        EmployeeId = employee.EmployeeId,
                        Name = employee.Name,
                        Email = employee.Email,
                        Role = employee.Role.Name
                    }
                };

                _logger.LogInformation("User logged in successfully.");
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
                _logger.LogError($"Error occurred: {ex.Message}");
            }
            return response;
        }

        public async Task<ResponseDto> Register(CreateEmployeeRequestDto requestDto)
        {
            return await _employeeService.CreateEmployeeAsync(requestDto);
        }

        private JwtSecurityToken GenerateToken(Data.Models.Employee employee)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expireMinutes = Convert.ToInt32(_configuration["Jwt:ExpireMinutes"]);

            var claims = new List<Claim>
                         {
                             new Claim(JwtRegisteredClaimNames.Sub, employee.EmployeeId.ToString()),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             new Claim(JwtRegisteredClaimNames.Email, employee.Email),
                             new Claim(ClaimTypes.Name, employee.Name),
                             new Claim(ClaimTypes.Role, employee.Role.Name)
                         };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                        issuer: issuer,
                        audience: audience,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(expireMinutes),
                        signingCredentials: credentials
                        );

            return token;
        }

    }
}
