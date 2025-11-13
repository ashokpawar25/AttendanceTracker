using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.AttendanceTracker.API.Controllers;
using Employee.AttendanceTracker.Business.DTOs;
using Employee.AttendanceTracker.Business.Services.Interfaces;
using Employee.AttendanceTracker.Data.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Employee.AttendanceTracker.Tests.Controller
{
    public class AttendanceControllerTests
    {
        private readonly Mock<IAttendanceService> _attendanceServiceMock;
        private readonly AttendanceController _controller;

        public AttendanceControllerTests()
        {
            _attendanceServiceMock = new Mock<IAttendanceService>();
            _controller = new AttendanceController(_attendanceServiceMock.Object);
        }

        [Fact]
        public async Task CreateAttendance_ShouldReturnOk_WhenAttendanceCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateAttendanceRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = true, Message = "Attendance created successfully" };

            _attendanceServiceMock.Setup(s => s.CreateAttendanceAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateAttendance(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAttendance_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var request = new CreateAttendanceRequestDto();
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Validation error" };

            _attendanceServiceMock.Setup(s => s.CreateAttendanceAsync(request))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateAttendance(request);

            // Assert
            var badResult = result as BadRequestObjectResult;
            badResult.Should().NotBeNull();
            var response = badResult.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Validation error");
        }

        [Fact]
        public async Task CreateAttendance_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var request = new CreateAttendanceRequestDto();
            _attendanceServiceMock.Setup(s => s.CreateAttendanceAsync(request))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateAttendance(request);

            // Assert
            var statusResult = result as ObjectResult;
            statusResult.Should().NotBeNull();
            statusResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAttendanceSummary_ShouldReturnOk_WhenSummaryRetrievedSuccessfully()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var date = DateTime.Now;
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new List<Attendance>() };

            _attendanceServiceMock.Setup(s => s.GetAttendanceSummaryByDepartmentAsync(departmentId, date))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAttendanceSummary(departmentId, date);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var response = okResult.Value as ResponseDto;
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetAttendanceSummary_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var date = DateTime.Now;
            var expectedResponse = new ResponseDto { IsSuccess = false, Message = "Invalid department" };

            _attendanceServiceMock.Setup(s => s.GetAttendanceSummaryByDepartmentAsync(departmentId, date))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAttendanceSummary(departmentId, date);

            // Assert
            var badResult = result as BadRequestObjectResult;
            badResult.Should().NotBeNull();
            var response = badResult.Value as ResponseDto;
            response.IsSuccess.Should().BeFalse();
            response.Message.Should().Be("Invalid department");
        }

        [Fact]
        public async Task GetAttendanceSummary_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var date = DateTime.Now;

            _attendanceServiceMock.Setup(s => s.GetAttendanceSummaryByDepartmentAsync(departmentId, date))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetAttendanceSummary(departmentId, date);

            // Assert
            var statusResult = result as ObjectResult;
            statusResult.Should().NotBeNull();
            statusResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DownloadAttendanceSummary_ShouldReturnFile_WhenDataExists()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var date = DateTime.Now;
            var csvBytes = Encoding.UTF8.GetBytes("EmployeeId,Name,Status\n1,John,Present");

            _attendanceServiceMock.Setup(s => s.GenerateAttendanceSummaryCsvAsync(departmentId, date))
                .ReturnsAsync(csvBytes);

            // Act
            var result = await _controller.DownloadAttendanceSummary(departmentId, date);

            // Assert
            var fileResult = result as FileContentResult;
            fileResult.Should().NotBeNull();
            fileResult.ContentType.Should().Be("text/csv");
            fileResult.FileDownloadName.Should().Contain("AttendanceSummary_");
        }

        [Fact]
        public async Task DownloadAttendanceSummary_ShouldReturnNotFound_WhenNoData()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var date = DateTime.Now;

            _attendanceServiceMock.Setup(s => s.GenerateAttendanceSummaryCsvAsync(departmentId, date))
                .ReturnsAsync(Array.Empty<byte>());

            // Act
            var result = await _controller.DownloadAttendanceSummary(departmentId, date);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DownloadAttendanceSummary_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var date = DateTime.Now;

            _attendanceServiceMock.Setup(s => s.GenerateAttendanceSummaryCsvAsync(departmentId, date))
                .ThrowsAsync(new Exception("CSV generation failed"));

            // Act
            var result = await _controller.DownloadAttendanceSummary(departmentId, date);

            // Assert
            var statusResult = result as ObjectResult;
            statusResult.Should().NotBeNull();
            statusResult.StatusCode.Should().Be(500);
        }
    }
}
