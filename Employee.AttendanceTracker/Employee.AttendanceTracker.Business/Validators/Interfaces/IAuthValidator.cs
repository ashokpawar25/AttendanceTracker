using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Validators.Interfaces
{
    public interface IAuthValidator
    {
        Task<List<string>> ValidateLoginAsync(LoginRequestDto requestDto);
    }
}
