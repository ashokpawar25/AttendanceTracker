using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ResponseDto> CreateRoleAsync(CreateRoleRequestDto requestDto);
        Task<ResponseDto> DeleteRoleAsync(Guid id);
        Task<ResponseDto> GetAllRolesAsync();
        Task<ResponseDto> GetRoleByIdAsync(Guid id);
        Task<ResponseDto> UpdateRoleAsync(Guid id, UpdateRoleRequestDto requestDto);
    }
}
