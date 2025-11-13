using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Implementations;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.AttendanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("create-employee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeRequestDto requestDto)
        {
            try
            {
                var response = await _employeeService.CreateEmployeeAsync(requestDto);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("get-all-employees")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var response = await _employeeService.GetAllEmployeesAsync();
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("get-employee/{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var response = await _employeeService.GetEmployeeByIdAsync(id);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("{id}/attendance")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> GetEmployeeAttendanceHistory(Guid id, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                var response = await _employeeService.GetEmployeeAttendanceHistoryAsync(id, FromDate, ToDate);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("{id}/attendance/download")]
        [Authorize(Roles = "Admin,HR,Employee")]
        public async Task<IActionResult> DownloadEmployeeAttendanceHistory(Guid id, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                var csvFile = await _employeeService.GenerateEmployeeAttendanceHistoryCsvAsync(id, FromDate, ToDate);

                if (csvFile == null || csvFile.Length == 0)
                    return NotFound(new { IsSuccess = false, Message = "No attendance records found for the given criteria." });

                var fileName = $"AttendanceSummary_{id}_{DateTime.Now:yyyyMMdd}.csv";
                return File(csvFile, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPut("update-employee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeRequestDto requestDto)
        {
            try
            {
                var response = await _employeeService.UpdateEmployeeAsync(id, requestDto);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete-employee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var response = await _employeeService.DeleteEmployeeAsync(id);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
