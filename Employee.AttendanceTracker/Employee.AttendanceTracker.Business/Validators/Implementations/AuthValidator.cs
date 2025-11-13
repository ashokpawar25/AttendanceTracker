using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace Employee.AttendanceTracker.Business.Validators.Implementations
{
    public class AuthValidator : IAuthValidator
    {
        private readonly AttendanceDbContext _dbContext;

        public AuthValidator(AttendanceDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<string>> ValidateLoginAsync(LoginRequestDto requestDto)
        {
            List<string> errors = new List<string>();

            if(!await _dbContext.Employees.AnyAsync(e => e.Email == requestDto.Email && e.Password == requestDto.Password))
            {
                errors.Add("Invalid email or password.");
            }

            return errors;
        }
    }
}
