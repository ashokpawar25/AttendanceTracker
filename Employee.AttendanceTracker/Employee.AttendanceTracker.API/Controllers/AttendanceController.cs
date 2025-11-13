using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.AttendanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("create-attendance")]
        [Authorize(Roles ="Admin,HR")]
        public async Task<IActionResult> CreateAttendance(CreateAttendanceRequestDto requestDto)
        {
            try
            {
                var response = await _attendanceService.CreateAttendanceAsync(requestDto);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("summary")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAttendanceSummary([FromQuery] Guid departmentId, [FromQuery] DateTime date)
        {
            try
            {
                var response = await _attendanceService.GetAttendanceSummaryByDepartmentAsync(departmentId, date);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("summary/download")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> DownloadAttendanceSummary([FromQuery] Guid departmentId, [FromQuery] DateTime date)
        {
            try
            {
                var csvFile = await _attendanceService.GenerateAttendanceSummaryCsvAsync(departmentId, date);

                if (csvFile == null || csvFile.Length == 0)
                    return NotFound(new { IsSuccess = false, Message = "No attendance records found for the given criteria." });

                var fileName = $"AttendanceSummary_{departmentId}_{date:yyyyMMdd}.csv";
                return File(csvFile, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
