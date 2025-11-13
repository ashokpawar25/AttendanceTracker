using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Data.Models;

namespace Employee.AttendanceTracker.Business.DTOs
{
    public class CreateAttendanceRequestDto
    {
        public Guid EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
    }
}
