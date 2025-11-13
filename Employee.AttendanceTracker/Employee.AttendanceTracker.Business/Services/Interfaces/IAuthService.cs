using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto> Login(LoginRequestDto requestDto);
        Task<ResponseDto> Register(CreateEmployeeRequestDto requestDto);
    }
}
