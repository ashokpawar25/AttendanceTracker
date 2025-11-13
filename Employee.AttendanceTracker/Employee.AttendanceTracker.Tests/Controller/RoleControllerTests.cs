using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Employee.AttendanceTracker.API.Controllers;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Employee.AttendanceTracker.Tests.Controller
{
    public class RoleControllerTests
    {
        private readonly Mock<IRoleService> _roleServiceMock;
        private readonly RoleController _controller;

        public RoleControllerTests()
        {
            _roleServiceMock = new Mock<IRoleService>();
            _controller = new RoleController(_roleServiceMock.Object);
        }

        [Fact]
        public async Task CreateRole_ShouldReturnOk_WhenRoleCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateRoleRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Role created successfully" };

            _roleServiceMock.Setup(s => s.CreateRoleAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateRole(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CreateRole_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new CreateRoleRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Validation error" };

            _roleServiceMock.Setup(s => s.CreateRoleAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateRole(request);

            // Assert
            var badResult = result as BadRequestObjectResult;
            badResult.Should().NotBeNull();
            var response = badResult.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation error");
        }

        [Fact]
        public async Task CreateRole_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateRoleRequestDto();
            _roleServiceMock.Setup(s => s.CreateRoleAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateRole(request);

            // Assert
            var statusResult = result as ObjectResult;
            statusResult.Should().NotBeNull();
            statusResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAllRoles_ShouldReturnOk_WhenRolesRetrievedSuccessfully()
        {
            // Arrange
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new List<Role>() };

            _roleServiceMock.Setup(s => s.GetAllRolesAsync())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllRoles();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllRoles_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Validation failed" };

            _roleServiceMock.Setup(s => s.GetAllRolesAsync())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllRoles();

            // Assert
            var badResult = result as BadRequestObjectResult;
            badResult.Should().NotBeNull();
            var response = badResult.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
        }

        [Fact]
        public async Task GetRoleById_ShouldReturnOk_WhenRoleRetrievedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new Role { RoleId = id } };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetRoleById(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateRole_ShouldReturnOk_WhenRoleUpdatedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateRoleRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Role updated successfully" };

            _roleServiceMock.Setup(s => s.UpdateRoleAsync(id, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UpdateRole(id, request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteRole_ShouldReturnOk_WhenRoleDeletedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Role deleted successfully" };

            _roleServiceMock.Setup(s => s.DeleteRoleAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DeleteRole(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteRole_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Cannot delete role" };

            _roleServiceMock.Setup(s => s.DeleteRoleAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DeleteRole(id);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            var response = badRequest.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
        }
    }
}
