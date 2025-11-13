using System;
using System.Collections.Generic;
using System.Text;
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
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_employeeServiceMock.Object);
        }

        [Fact]
        public async Task CreateEmployee_ShouldReturnOk_WhenEmployeeCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Employee created successfully" };

            _employeeServiceMock.Setup(s => s.CreateEmployeeAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CreateEmployee_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Validation error" };

            _employeeServiceMock.Setup(s => s.CreateEmployeeAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            var badResult = result as BadRequestObjectResult;
            badResult.Should().NotBeNull();
            var response = badResult.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation error");
        }

        [Fact]
        public async Task CreateEmployee_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateEmployeeRequestDto();
            _employeeServiceMock.Setup(s => s.CreateEmployeeAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            var statusResult = result as ObjectResult;
            statusResult.Should().NotBeNull();
            statusResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAllEmployees_ShouldReturnOk_WhenEmployeesRetrievedSuccessfully()
        {
            // Arrange
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new List<Data.Models.Employee>() };

            _employeeServiceMock.Setup(s => s.GetAllEmployeesAsync())
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAllEmployees();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnOk_WhenEmployeeRetrievedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new Data.Models.Employee { EmployeeId = id } };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetEmployeeById(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetEmployeeAttendanceHistory_ShouldReturnOk_WhenHistoryRetrievedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new List<Attendance>() };

            _employeeServiceMock.Setup(s => s.GetEmployeeAttendanceHistoryAsync(id, null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetEmployeeAttendanceHistory(id, null, null);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DownloadEmployeeAttendanceHistory_ShouldReturnFile_WhenDataExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var csvBytes = Encoding.UTF8.GetBytes("EmployeeId,Name,Status\n1,John,Present");

            _employeeServiceMock.Setup(s => s.GenerateEmployeeAttendanceHistoryCsvAsync(id, null, null))
                .ReturnsAsync(csvBytes);

            // Act
            var result = await _controller.DownloadEmployeeAttendanceHistory(id, null, null);

            // Assert
            var fileResult = result as FileContentResult;
            fileResult.Should().NotBeNull();
            fileResult.ContentType.Should().Be("text/csv");
            fileResult.FileDownloadName.Should().Contain("AttendanceSummary_");
        }

        [Fact]
        public async Task DownloadEmployeeAttendanceHistory_ShouldReturnNotFound_WhenNoData()
        {
            // Arrange
            var id = Guid.NewGuid();
            _employeeServiceMock.Setup(s => s.GenerateEmployeeAttendanceHistoryCsvAsync(id, null, null))
                .ReturnsAsync(Array.Empty<byte>());

            // Act
            var result = await _controller.DownloadEmployeeAttendanceHistory(id, null, null);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateEmployee_ShouldReturnOk_WhenEmployeeUpdatedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateEmployeeRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Employee updated successfully" };

            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(id, request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UpdateEmployee(id, request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnOk_WhenEmployeeDeletedSuccessfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Employee deleted successfully" };

            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DeleteEmployee(id);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Cannot delete employee" };

            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(id))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DeleteEmployee(id);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            var response = badRequest.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
        }
    }
}
