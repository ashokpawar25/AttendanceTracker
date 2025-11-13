using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.AttendanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {

            this._authService = authService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateEmployeeRequestDto requestDto)
        {
            try
            {
                var response = await _authService.Register(requestDto);
                if (response.IsSuccess)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto requestDto)
        {
            try
            {
                var response = await _authService.Login(requestDto);
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
