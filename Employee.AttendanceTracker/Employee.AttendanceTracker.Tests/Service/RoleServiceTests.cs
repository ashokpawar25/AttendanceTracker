using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Implementations;
using Employee.AttendanceTracker.Business.Validators.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using Employee.AttendanceTracker.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Employee.AttendanceTracker.Tests.Service
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IRoleValidator> _roleValidatorMock;
        private readonly Mock<ILogger<RoleService>> _loggerMock;
        private readonly RoleService _service;

        public RoleServiceTests()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _roleValidatorMock = new Mock<IRoleValidator>();
            _loggerMock = new Mock<ILogger<RoleService>>();
            _service = new RoleService(_roleRepositoryMock.Object, _roleValidatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateRoleAsync_ShouldReturnSuccess_WhenValidationPasses()
        {
            // Arrange
            var request = new CreateRoleRequestDto { RoleName = "Admin" };
            _roleValidatorMock.Setup(v => v.ValidateCreateRoleAsync(request))
                .ReturnsAsync(new List<string>());
            _roleRepositoryMock.Setup(r => r.CreateRoleAsync(It.IsAny<Role>()))
                .ReturnsAsync(new Role { RoleId = Guid.NewGuid() });

            // Act
            var result = await _service.CreateRoleAsync(request);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Message.ShouldBe("Role created successfully.");
            result.Result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateRoleAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var request = new CreateRoleRequestDto { RoleName = "" };
            _roleValidatorMock.Setup(v => v.ValidateCreateRoleAsync(request))
                .ReturnsAsync(new List<string> { "Role name is required" });

            // Act
            var result = await _service.CreateRoleAsync(request);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Role name is required");
        }

        [Fact]
        public async Task CreateRoleAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateRoleRequestDto { RoleName = "Finance" };
            _roleValidatorMock.Setup(v => v.ValidateCreateRoleAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.CreateRoleAsync(request);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldContain("Database error");
        }

        [Fact]
        public async Task GetAllRolesAsync_ShouldReturnRolesSuccessfully()
        {
            // Arrange
            var roles = new List<Role>
            {
                new Role { RoleId = Guid.NewGuid(), Name = "Admin" },
                new Role { RoleId = Guid.NewGuid(), Name = "HR" }
            };
            _roleRepositoryMock.Setup(r => r.GetAllRolesAsync())
                .ReturnsAsync(roles);

            // Act
            var result = await _service.GetAllRolesAsync();

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Result.ShouldBe(roles);
        }

        [Fact]
        public async Task GetAllRolesAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            _roleRepositoryMock.Setup(r => r.GetAllRolesAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _service.GetAllRolesAsync();

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldContain("Database error");
        }

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnRole_WhenValidationPasses()
        {
            // Arrange
            var id = Guid.NewGuid();
            _roleValidatorMock.Setup(v => v.ValidateGetRoleByIdAsync(id))
                .ReturnsAsync(new List<string>());
            var role = new Role { RoleId = id, Name = "Admin" };
            _roleRepositoryMock.Setup(r => r.GetRoleByIdAsync(id))
                .ReturnsAsync(role);

            // Act
            var result = await _service.GetRoleByIdAsync(id);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Result.ShouldBe(role);
        }

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _roleValidatorMock.Setup(v => v.ValidateGetRoleByIdAsync(id))
                .ReturnsAsync(new List<string> { "Invalid ID" });

            // Act
            var result = await _service.GetRoleByIdAsync(id);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Invalid ID");
        }

        [Fact]
        public async Task UpdateRoleAsync_ShouldReturnSuccess_WhenValidationPasses()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateRoleRequestDto { RoleName = "HR" };
            _roleValidatorMock.Setup(v => v.ValidateUpdateRoleAsync(id, request))
                .ReturnsAsync(new List<string>());
            var existingRole = new Role { RoleId = id, Name = "Admin" };
            _roleRepositoryMock.Setup(r => r.GetRoleByIdAsync(id))
                .ReturnsAsync(existingRole);

            // Act
            var result = await _service.UpdateRoleAsync(id, request);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Result.ShouldBe(existingRole);
            existingRole.Name.ShouldBe("HR");
        }

        [Fact]
        public async Task UpdateRoleAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateRoleRequestDto { RoleName = "" };
            _roleValidatorMock.Setup(v => v.ValidateUpdateRoleAsync(id, request))
                .ReturnsAsync(new List<string> { "Role name required" });

            // Act
            var result = await _service.UpdateRoleAsync(id, request);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Role name required");
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnSuccess_WhenValidationPasses()
        {
            // Arrange
            var id = Guid.NewGuid();
            _roleValidatorMock.Setup(v => v.ValidateDeleteRoleAsync(id))
                .ReturnsAsync(new List<string>());
            var existingRole = new Role { RoleId = id, Name = "Admin" };
            _roleRepositoryMock.Setup(r => r.GetRoleByIdAsync(id))
                .ReturnsAsync(existingRole);

            // Act
            var result = await _service.DeleteRoleAsync(id);

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _roleValidatorMock.Setup(v => v.ValidateDeleteRoleAsync(id))
                .ReturnsAsync(new List<string> { "Cannot delete role" });

            // Act
            var result = await _service.DeleteRoleAsync(id);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Cannot delete role");
        }
    }
}
