using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Implementations;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Employee.AttendanceTracker.Tests.Service
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly Mock<IEmployeeValidator> _mockValidator;
        private readonly Mock<IAttendanceService> _mockAttendanceService;
        private readonly Mock<ILogger<EmployeeService>> _mockLogger;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _mockValidator = new Mock<IEmployeeValidator>();
            _mockAttendanceService = new Mock<IAttendanceService>();
            _mockLogger = new Mock<ILogger<EmployeeService>>();

            _service = new EmployeeService(
                _mockRepo.Object,
                _mockValidator.Object,
                _mockAttendanceService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto { Name = "John", Email = "john@example.com" };
            _mockValidator.Setup(v => v.ValidateCreateEmployeeAsync(request)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.CreateEmployeeAsync(It.IsAny<Data.Models.Employee>()))
                     .ReturnsAsync(new Data.Models.Employee { EmployeeId = Guid.NewGuid() });

            // Act
            var result = await _service.CreateEmployeeAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Employee created successfully.");
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto();
            _mockValidator.Setup(v => v.ValidateCreateEmployeeAsync(request))
                          .ReturnsAsync(new List<string> { "Invalid name" });

            // Act
            var result = await _service.CreateEmployeeAsync(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Invalid name");
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnError_WhenRepositoryThrows()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto { Name = "John" };
            _mockValidator.Setup(v => v.ValidateCreateEmployeeAsync(request)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.CreateEmployeeAsync(It.IsAny<Data.Models.Employee>()))
                     .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.CreateEmployeeAsync(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Database error");
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllEmployeesAsync())
                     .ReturnsAsync(new List<Data.Models.Employee> { new() });

            // Act
            var result = await _service.GetAllEmployeesAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            ((List<Data.Models.Employee>)result.Result).Should().HaveCount(1);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ShouldReturnError_WhenRepositoryThrows()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllEmployeesAsync())
                     .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.GetAllEmployeesAsync();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Database error");
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnSuccess_WhenValidId()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeByIdAsync(id)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync(id))
                     .ReturnsAsync(new Data.Models.Employee { Name = "John" });

            // Act
            var result = await _service.GetEmployeeByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Employee retrieved successfully.");
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeByIdAsync(id))
                          .ReturnsAsync(new List<string> { "Invalid ID" });

            // Act
            var result = await _service.GetEmployeeByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Invalid ID");
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnError_WhenRepositoryThrows()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeByIdAsync(id)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync(id))
                     .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _service.GetEmployeeByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("DB error");
        }

        [Fact]
        public async Task GetEmployeeByEmailAsync_ShouldReturnSuccess_WhenValidEmail()
        {
            // Arrange
            var email = "john@example.com";
            _mockValidator.Setup(v => v.ValidateGetEmployeeByEmailAsync(email)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByEmailAsync(email))
                     .ReturnsAsync(new Data.Models.Employee { Email = email });

            // Act
            var result = await _service.GetEmployeeByEmailAsync(email);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Employee retrieved successfully.");
        }

        [Fact]
        public async Task GetEmployeeByEmailAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var email = "invalidemail";
            _mockValidator.Setup(v => v.ValidateGetEmployeeByEmailAsync(email))
                          .ReturnsAsync(new List<string> { "Invalid email" });

            // Act
            var result = await _service.GetEmployeeByEmailAsync(email);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Invalid email");
        }

        [Fact]
        public async Task GetEmployeeByEmailAsync_ShouldReturnError_WhenRepositoryThrows()
        {
            // Arrange
            var email = "john@example.com";
            _mockValidator.Setup(v => v.ValidateGetEmployeeByEmailAsync(email)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByEmailAsync(email))
                     .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _service.GetEmployeeByEmailAsync(email);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("DB error");
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnSuccess_WhenValidData()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateEmployeeRequestDto { Name = "Updated" };

            _mockValidator.Setup(v => v.ValidateUpdateEmployeeAsync(id, request)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync(id))
                     .ReturnsAsync(new Data.Models.Employee());
            _mockRepo.Setup(r => r.UpdateEmployeeAsync(It.IsAny<Data.Models.Employee>()))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateEmployeeAsync(id, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Employee updated successfully.");
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateEmployeeRequestDto();
            _mockValidator.Setup(v => v.ValidateUpdateEmployeeAsync(id, request))
                          .ReturnsAsync(new List<string> { "Invalid ID" });

            // Act
            var result = await _service.UpdateEmployeeAsync(id, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Invalid ID");
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnError_WhenRepositoryThrows()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateEmployeeRequestDto { Name = "John" };

            _mockValidator.Setup(v => v.ValidateUpdateEmployeeAsync(id, request)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync(id))
                     .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _service.UpdateEmployeeAsync(id, request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("DB error");
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnSuccess_WhenValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateDeleteEmployeeAsync(id)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync(id))
                     .ReturnsAsync(new Data.Models.Employee());
            _mockRepo.Setup(r => r.DeleteEmployeeAsync(It.IsAny<Data.Models.Employee>()))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteEmployeeAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Employee deleted successfully.");
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateDeleteEmployeeAsync(id))
                          .ReturnsAsync(new List<string> { "Invalid ID" });

            // Act
            var result = await _service.DeleteEmployeeAsync(id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Invalid ID");
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldReturnError_WhenRepositoryThrows()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateDeleteEmployeeAsync(id)).ReturnsAsync(new List<string>());
            _mockRepo.Setup(r => r.GetEmployeeByIdAsync(id))
                     .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _service.DeleteEmployeeAsync(id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("DB error");
        }

        [Fact]
        public async Task GetEmployeeAttendanceHistoryAsync_ShouldReturnSuccess_WhenValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeAttendanceHistoryAsync(id)).ReturnsAsync(new List<string>());
            _mockAttendanceService.Setup(a => a.GetEmployeeAttendanceHistoryAsync(id, null, null))
                                  .ReturnsAsync(new ResponseDto 
                                  { 
                                      IsSuccess = true, 
                                      Result = new List<Attendance> { new Attendance() }
                                  });

            // Act
            var result = await _service.GetEmployeeAttendanceHistoryAsync(id, null, null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Result.Should().BeOfType<List<Attendance>>();
        }

        [Fact]
        public async Task GetEmployeeAttendanceHistoryAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeAttendanceHistoryAsync(id))
                          .ReturnsAsync(new List<string> { "Invalid ID" });

            // Act
            var result = await _service.GetEmployeeAttendanceHistoryAsync(id, null, null);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Invalid ID");
        }

        [Fact]
        public async Task GetEmployeeAttendanceHistoryAsync_ShouldReturnError_WhenAttendanceServiceThrows()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeAttendanceHistoryAsync(id)).ReturnsAsync(new List<string>());
            _mockAttendanceService.Setup(a => a.GetEmployeeAttendanceHistoryAsync(id, null, null))
                                  .ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _service.GetEmployeeAttendanceHistoryAsync(id, null, null);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Service error");
        }

        [Fact]
        public async Task GenerateEmployeeAttendanceHistoryCsvAsync_ShouldReturnCsvBytes_WhenDataExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeAttendanceHistoryAsync(id)).ReturnsAsync(new List<string>());

            var attendances = new List<Attendance>
                    {
                        new Attendance
                        {
                            Employee = new Data.Models.Employee
                            {
                                EmployeeId = id, Name = "John", Department = new Department { Name = "IT" }
                            },
                            AttendanceDate = DateTime.Today,
                            AttendanceStatus = AttendanceStatus.PRESENT
                        }
                    };

            _mockAttendanceService.Setup(a => a.GetEmployeeAttendanceHistoryAsync(id, null, null))
                                  .ReturnsAsync(new ResponseDto { Result = attendances, IsSuccess = true });

            // Act
            var csvBytes = await _service.GenerateEmployeeAttendanceHistoryCsvAsync(id, null, null);

            // Assert
            csvBytes.Should().NotBeEmpty();
            Encoding.UTF8.GetString(csvBytes).Should().Contain("John");
        }

        [Fact]
        public async Task GenerateEmployeeAttendanceHistoryCsvAsync_ShouldReturnEmpty_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeAttendanceHistoryAsync(id))
                          .ReturnsAsync(new List<string> { "Invalid ID" });

            // Act
            var csvBytes = await _service.GenerateEmployeeAttendanceHistoryCsvAsync(id, null, null);

            // Assert
            csvBytes.Should().BeEmpty();
        }

        [Fact]
        public async Task GenerateEmployeeAttendanceHistoryCsvAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeAttendanceHistoryAsync(id)).ReturnsAsync(new List<string>());
            _mockAttendanceService.Setup(a => a.GetEmployeeAttendanceHistoryAsync(id, null, null))
                                  .ReturnsAsync(new ResponseDto { Result = new List<Attendance>() });

            // Act
            var csvBytes = await _service.GenerateEmployeeAttendanceHistoryCsvAsync(id, null, null);

            // Assert
            csvBytes.Should().BeEmpty();
        }

        [Fact]
        public async Task GenerateEmployeeAttendanceHistoryCsvAsync_ShouldThrow_WhenExceptionOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockValidator.Setup(v => v.ValidateGetEmployeeAttendanceHistoryAsync(id)).ReturnsAsync(new List<string>());
            _mockAttendanceService.Setup(a => a.GetEmployeeAttendanceHistoryAsync(id, null, null))
                                  .ThrowsAsync(new Exception("Error"));

            // Act
            Func<Task> act = async () => await _service.GenerateEmployeeAttendanceHistoryCsvAsync(id, null, null);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Error");
        }

    }
}
