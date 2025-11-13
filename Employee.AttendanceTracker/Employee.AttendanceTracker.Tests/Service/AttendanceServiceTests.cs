using System.Text;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Implementations;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Employee.AttendanceTracker.Tests.Service
{
    public class AttendanceServiceTests
    {
        private readonly Mock<IAttendanceRepository> _mockRepo;
        private readonly Mock<IAttendanceValidator> _mockValidator;
        private readonly Mock<ILogger<AttendanceService>> _mockLogger;
        private readonly AttendanceService _service;

        public AttendanceServiceTests()
        {
            _mockRepo = new Mock<IAttendanceRepository>();
            _mockValidator = new Mock<IAttendanceValidator>();
            _mockLogger = new Mock<ILogger<AttendanceService>>();

            _service = new AttendanceService(_mockRepo.Object, _mockValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateAttendanceAsync_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var request = new CreateAttendanceRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                AttendanceDate = DateTime.Today,
                AttendanceStatus = AttendanceStatus.PRESENT
            };

            _mockValidator.Setup(v => v.ValidateCreateAttendanceAsync(request))
                          .ReturnsAsync(new List<string>());

            _mockRepo.Setup(r => r.CreateAttendanceAsync(It.IsAny<Attendance>()))
                     .ReturnsAsync(new Attendance { AttendanceId = Guid.NewGuid() });

            // Act
            var result = await _service.CreateAttendanceAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("Attendance created successfully.");
        }

        [Fact]
        public async Task CreateAttendanceAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var request = new CreateAttendanceRequestDto();
            _mockValidator.Setup(v => v.ValidateCreateAttendanceAsync(request))
                          .ReturnsAsync(new List<string> { "Duplicate attendance found" });

            // Act
            var result = await _service.CreateAttendanceAsync(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Duplicate attendance found");
        }

        [Fact]
        public async Task CreateAttendanceAsync_ShouldReturnError_WhenRepositoryThrowsException()
        {
            // Arrange
            var request = new CreateAttendanceRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                AttendanceDate = DateTime.Today,
                AttendanceStatus = AttendanceStatus.PRESENT
            };

            _mockValidator.Setup(v => v.ValidateCreateAttendanceAsync(request))
                          .ReturnsAsync(new List<string>());

            _mockRepo.Setup(r => r.CreateAttendanceAsync(It.IsAny<Attendance>()))
                     .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.CreateAttendanceAsync(request);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Database error");
        }

        [Fact]
        public async Task GetAttendanceSummaryByDepartmentAsync_ShouldReturnSuccess_WhenValid()
        {
            // Arrange
            var deptId = Guid.NewGuid();
            var date = DateTime.Today;

            _mockValidator.Setup(v => v.ValidateGetAttendanceSummaryByDepartmentAsync(deptId, date))
                          .ReturnsAsync(new List<string>());

            _mockRepo.Setup(r => r.GetAttendanceSummaryByDepartmentAsync(deptId, date))
                     .ReturnsAsync(new List<Attendance> { new Attendance() });

            // Act
            var result = await _service.GetAttendanceSummaryByDepartmentAsync(deptId, date);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Result.Should().BeOfType<List<Attendance>>();
        }

        [Fact]
        public async Task GetAttendanceSummaryByDepartmentAsync_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var deptId = Guid.NewGuid();
            var date = DateTime.Today;

            _mockValidator.Setup(v => v.ValidateGetAttendanceSummaryByDepartmentAsync(deptId, date))
                          .ReturnsAsync(new List<string> { "Invalid department" });

            // Act
            var result = await _service.GetAttendanceSummaryByDepartmentAsync(deptId, date);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Invalid department");
        }

        [Fact]
        public async Task GetAttendanceSummaryByDepartmentAsync_ShouldReturnEmptyList_WhenNoData()
        {
            // Arrange
            var deptId = Guid.NewGuid();
            var date = DateTime.Today;

            _mockValidator.Setup(v => v.ValidateGetAttendanceSummaryByDepartmentAsync(deptId, date))
                          .ReturnsAsync(new List<string>());

            _mockRepo.Setup(r => r.GetAttendanceSummaryByDepartmentAsync(deptId, date))
                     .ReturnsAsync(new List<Attendance>());

            // Act
            var result = await _service.GetAttendanceSummaryByDepartmentAsync(deptId, date);

            // Assert
            result.IsSuccess.Should().BeTrue();
            ((List<Attendance>)result.Result).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAttendanceSummaryByDepartmentAsync_ShouldReturnError_WhenRepositoryThrowsException()
        {
            // Arrange
            var deptId = Guid.NewGuid();
            var date = DateTime.Today;

            _mockValidator.Setup(v => v.ValidateGetAttendanceSummaryByDepartmentAsync(deptId, date))
                          .ReturnsAsync(new List<string>());

            _mockRepo.Setup(r => r.GetAttendanceSummaryByDepartmentAsync(deptId, date))
                     .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.GetAttendanceSummaryByDepartmentAsync(deptId, date);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Database error");
        }

        [Fact]
        public async Task GetEmployeeAttendanceHistoryAsync_ShouldReturnSuccess_WhenValid()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetEmployeeAttendanceHistoryAsync(employeeId, null, null))
                     .ReturnsAsync(new List<Attendance> { new Attendance() });

            // Act
            var result = await _service.GetEmployeeAttendanceHistoryAsync(employeeId, null, null);

            result.IsSuccess.Should().BeTrue();
            result.Result.Should().BeOfType<List<Attendance>>();
        }

        [Fact]
        public async Task GetEmployeeAttendanceHistoryAsync_ShouldReturnEmptyList_WhenNoHistory()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetEmployeeAttendanceHistoryAsync(employeeId, null, null))
                     .ReturnsAsync(new List<Attendance>());

            // Act
            var result = await _service.GetEmployeeAttendanceHistoryAsync(employeeId, null, null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            ((List<Attendance>)result.Result).Should().BeEmpty();
        }

        [Fact]
        public async Task GetEmployeeAttendanceHistoryAsync_ShouldReturnError_WhenRepositoryThrowsException()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetEmployeeAttendanceHistoryAsync(employeeId, null, null))
                     .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.GetEmployeeAttendanceHistoryAsync(employeeId, null, null);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Contain("Database error");
        }

        [Fact]
        public async Task GenerateAttendanceSummaryCsvAsync_ShouldReturnCsvBytes_WhenDataExists()
        {
            // Arrange
            var deptId = Guid.NewGuid();
            var date = DateTime.Today;
            var attendances = new List<Attendance>
                    {
                        new Attendance
                        {
                            Employee = new Data.Models.Employee { EmployeeId = Guid.NewGuid(), Name = "John", Department = new Department { Name = "IT" } },
                            AttendanceDate = date,
                            AttendanceStatus = AttendanceStatus.PRESENT
                        }
                    };

            _mockRepo.Setup(r => r.GetAttendanceSummaryByDepartmentAsync(deptId, date))
                     .ReturnsAsync(attendances);

            // Act
            var csvBytes = await _service.GenerateAttendanceSummaryCsvAsync(deptId, date);

            // Assert
            csvBytes.Should().NotBeEmpty();
            var csvString = Encoding.UTF8.GetString(csvBytes);
            csvString.Should().Contain("EmployeeId,EmployeeName,DepartmentName,Date,Status");
            csvString.Should().Contain("John");
        }

        [Fact]
        public async Task GenerateAttendanceSummaryCsvAsync_ShouldReturnEmptyBytes_WhenNoData()
        {
            // Arrange
            var deptId = Guid.NewGuid();
            var date = DateTime.Today;

            _mockRepo.Setup(r => r.GetAttendanceSummaryByDepartmentAsync(deptId, date))
                     .ReturnsAsync(new List<Attendance>());

            // Act
            var csvBytes = await _service.GenerateAttendanceSummaryCsvAsync(deptId, date);

            // Assert
            csvBytes.Should().BeEmpty();
        }

        [Fact]
        public async Task GenerateAttendanceSummaryCsvAsync_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var deptId = Guid.NewGuid();
            var date = DateTime.Today;

            _mockRepo.Setup(r => r.GetAttendanceSummaryByDepartmentAsync(deptId, date))
                     .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await _service.GenerateAttendanceSummaryCsvAsync(deptId, date);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
        }
    }
}