using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.AttendanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this._departmentService = departmentService;
        }

        [HttpPost("create-department")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentRequestDto requestDto)
        {
            try
            {
                var response = await _departmentService.CreateDepartmentAsync(requestDto);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("get-all-departments")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var response = await _departmentService.GetAllDepartmentsAsync();
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("get-department/{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            try
            {
                var response = await _departmentService.GetDepartmentByIdAsync(id);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPut("update-department/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment(Guid id, UpdateDepartmentRequestDto requestDto)
        {
            try
            {
                var response = await _departmentService.UpdateDepartmentAsync(id, requestDto);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete-department/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            try
            {
                var response = await _departmentService.DeleteDepartmentAsync(id);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
