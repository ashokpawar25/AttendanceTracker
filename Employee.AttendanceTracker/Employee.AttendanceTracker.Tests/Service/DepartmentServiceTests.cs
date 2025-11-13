using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Implementations;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Employee.AttendanceTracker.Tests.Service
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentRepository> _departmentRepositoryMock;
        private readonly Mock<IDepartmentValidator> _departmentValidatorMock;
        private readonly Mock<ILogger<DepartmentService>> _loggerMock;
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            _departmentRepositoryMock = new Mock<IDepartmentRepository>();
            _departmentValidatorMock = new Mock<IDepartmentValidator>();
            _loggerMock = new Mock<ILogger<DepartmentService>>();

            _service = new DepartmentService(
                _departmentRepositoryMock.Object,
                _departmentValidatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateDepartmentAsync_ShouldReturnSuccess_WhenValidationPasses()
        {
            // Arrange
            var request = new CreateDepartmentRequestDto { DepartmentName = "HR" };
            _departmentValidatorMock.Setup(v => v.ValidateCreateDepartmentAsync(request))
                .ReturnsAsync(new List<string>());
            _departmentRepositoryMock.Setup(r => r.CreateDepartmentAsync(It.IsAny<Department>()))
                .ReturnsAsync(new Department { DepartmentId = Guid.NewGuid() });

            // Act
            var result = await _service.CreateDepartmentAsync(request);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Message.ShouldBe("Department created successfully.");
            result.Result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateDepartmentAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var request = new CreateDepartmentRequestDto { DepartmentName = "" };
            _departmentValidatorMock.Setup(v => v.ValidateCreateDepartmentAsync(request))
                .ReturnsAsync(new List<string> { "Department name is required" });

            // Act
            var result = await _service.CreateDepartmentAsync(request);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Department name is required");
        }

        [Fact]
        public async Task CreateDepartmentAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateDepartmentRequestDto { DepartmentName = "Finance" };
            _departmentValidatorMock.Setup(v => v.ValidateCreateDepartmentAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.CreateDepartmentAsync(request);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldContain("Database error");
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnDepartmentsSuccessfully()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { DepartmentId = Guid.NewGuid(), Name = "HR" },
                new Department { DepartmentId = Guid.NewGuid(), Name = "Finance" }
            };
            _departmentRepositoryMock.Setup(r => r.GetAllDepartmentsAsync())
                .ReturnsAsync(departments);

            // Act
            var result = await _service.GetAllDepartmentsAsync();

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Result.ShouldBe(departments);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            _departmentRepositoryMock.Setup(r => r.GetAllDepartmentsAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.GetAllDepartmentsAsync();

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldContain("Database error");
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ShouldReturnDepartment_WhenValidationPasses()
        {
            // Arrange
            var id = Guid.NewGuid();
            _departmentValidatorMock.Setup(v => v.ValidateGetDepartmentByIdAsync(id))
                .ReturnsAsync(new List<string>());
            var department = new Department { DepartmentId = id, Name = "HR" };
            _departmentRepositoryMock.Setup(r => r.GetDepartmentByIdAsync(id))
                .ReturnsAsync(department);

            // Act
            var result = await _service.GetDepartmentByIdAsync(id);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Result.ShouldBe(department);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _departmentValidatorMock.Setup(v => v.ValidateGetDepartmentByIdAsync(id))
                .ReturnsAsync(new List<string> { "Invalid ID" });

            // Act
            var result = await _service.GetDepartmentByIdAsync(id);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Invalid ID");
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ShouldReturnSuccess_WhenValidationPasses()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateDepartmentRequestDto { DepartmentName = "Finance" };
            _departmentValidatorMock.Setup(v => v.ValidateUpdateDepartmentAsync(id, request))
                .ReturnsAsync(new List<string>());
            var department = new Department { DepartmentId = id, Name = "HR" };
            _departmentRepositoryMock.Setup(r => r.GetDepartmentByIdAsync(id))
                .ReturnsAsync(department);

            // Act
            var result = await _service.UpdateDepartmentAsync(id, request);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Result.ShouldBe(department);
            department.Name.ShouldBe("Finance");
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateDepartmentRequestDto { DepartmentName = "" };
            _departmentValidatorMock.Setup(v => v.ValidateUpdateDepartmentAsync(id, request))
                .ReturnsAsync(new List<string> { "Department name required" });

            // Act
            var result = await _service.UpdateDepartmentAsync(id, request);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Department name required");
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ShouldReturnSuccess_WhenValidationPasses()
        {
            // Arrange
            var id = Guid.NewGuid();
            _departmentValidatorMock.Setup(v => v.ValidateDeleteDepartmentAsync(id))
                .ReturnsAsync(new List<string>());
            var department = new Department { DepartmentId = id, Name = "HR" };
            _departmentRepositoryMock.Setup(r => r.GetDepartmentByIdAsync(id))
                .ReturnsAsync(department);

            // Act
            var result = await _service.DeleteDepartmentAsync(id);

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _departmentValidatorMock.Setup(v => v.ValidateDeleteDepartmentAsync(id))
                .ReturnsAsync(new List<string> { "Cannot delete department" });

            // Act
            var result = await _service.DeleteDepartmentAsync(id);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Cannot delete department");
        }
    }
}
