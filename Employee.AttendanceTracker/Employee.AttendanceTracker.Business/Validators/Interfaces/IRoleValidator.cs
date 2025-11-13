using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;

namespace Employee.AttendanceTracker.Business.Validators.Interfaces
{
    public interface IRoleValidator
    {
        Task<List<string>> ValidateCreateRoleAsync(CreateRoleRequestDto requestDto);
        Task<List<string>> ValidateDeleteRoleAsync(Guid id);
        Task<List<string>> ValidateGetRoleByIdAsync(Guid id);
        Task<List<string>> ValidateUpdateRoleAsync(Guid id, UpdateRoleRequestDto requestDto);
    }
}
