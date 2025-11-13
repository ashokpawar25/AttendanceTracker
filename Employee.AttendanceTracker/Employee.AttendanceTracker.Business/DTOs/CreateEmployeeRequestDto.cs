using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.AttendanceTracker.Business.DTOs
{
    public class CreateEmployeeRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid RoleId { get; set; }
    }
}
