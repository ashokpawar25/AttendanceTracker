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
    public class DepartmentControllerTests
    {
        private readonly Mock<IDepartmentService> _departmentServiceMock;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _departmentServiceMock = new Mock<IDepartmentService>();
            _controller = new DepartmentController(_departmentServiceMock.Object);
        }

        [Fact]
        public async Task CreateDepartment_ShouldReturnOk_WhenDepartmentCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateDepartmentRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Department created successfully" };

            _departmentServiceMock.Setup(s => s.CreateDepartmentAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateDepartment(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CreateDepartment_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new CreateDepartmentRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Validation failed" };

            _departmentServiceMock.Setup(s => s.CreateDepartmentAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateDepartment(request);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            var response = badRequest.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
        }

        [Fact]
        public async Task CreateDepartment_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateDepartmentRequestDto();
            _departmentServiceMock.Setup(s => s.CreateDepartmentAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateDepartment(request);

            // Assert
            var statusResult = result as ObjectResult;
            statusResult.Should().NotBeNull();
            statusResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAllDepartments_ShouldReturnOk_WhenDepartmentsRetrievedSuccessfully()
        {
            // Arrange
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new List<Department>() };

            _departmentServiceMock.Setup(s => s.GetAllDepartmentsAsync())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllDepartments();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllDepartments_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Validation failed" };

            _departmentServiceMock.Setup(s => s.GetAllDepartmentsAsync())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllDepartments();

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            var response = badRequest.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation failed");
        }

        [Fact]
        public async Task GetDepartmentById_ShouldReturnOk_WhenDepartmentRetrievedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new Department { DepartmentId = id } };

            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDepartmentById(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateDepartment_ShouldReturnOk_WhenDepartmentUpdatedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateDepartmentRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Department updated successfully" };

            _departmentServiceMock.Setup(s => s.UpdateDepartmentAsync(id, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UpdateDepartment(id, request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteDepartment_ShouldReturnOk_WhenDepartmentDeletedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Department deleted successfully" };

            _departmentServiceMock.Setup(s => s.DeleteDepartmentAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DeleteDepartment(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteDepartment_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Cannot delete department" };

            _departmentServiceMock.Setup(s => s.DeleteDepartmentAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DeleteDepartment(id);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            var response = badRequest.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Cannot delete department");
        }
    }
}
