using EmployeeHierarchy.Application.DTOs;
using EmployeeHierarchy.Application.Interfaces;
using EmployeeHierarchy.Domain.Entities;
using EmployeeHierarchyApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;

namespace EmployeeHierarchyApi.Tests
{
    public class EmployeeControllerTests
    {

        private Mock<IEmployeeService> mockEmployeeService;
        private EmployeeController employeeController;
        public EmployeeControllerTests()
        {
            mockEmployeeService = new Mock<IEmployeeService>();
            employeeController = new EmployeeController(mockEmployeeService.Object);

            var expectedHierarchy = new List<EmployeeDto>
            {
                new EmployeeDto
                {
                    EmployeeId = 1,
                    Name = "Alice",
                    Reports = new List<EmployeeDto>
                    {
                        new EmployeeDto { EmployeeId = 2, Name = "Bob", Reports = new() }
                    }
                }
            };

            mockEmployeeService.Setup(s => s.BuildHierarchyAsync(It.IsAny<IFormFile>()))
                       .ReturnsAsync(expectedHierarchy);
        }
        [Fact]
        public async Task Upload_ValidFile_ReturnsOkWithData()
        {
            // Arrange           
            var csvContent = "Employee Id,Employee Name,Manager Id\n1,Alice,\n2,Bob,1";
            var bytes = Encoding.UTF8.GetBytes(csvContent);
            var stream = new MemoryStream(bytes);
            var file = new FormFile(stream, 0, bytes.Length, "file", "employees.csv");

            // Act
            var result = await employeeController.UploadFile(file);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            // Use reflection to get 'message' property
            var prop = objectResult.Value!.GetType().GetProperty("message");
            Assert.NotNull(prop);
            var value = prop!.GetValue(objectResult.Value);
            // Assert it's a list of EmployeeDto
            var employeeList = Assert.IsType<List<EmployeeDto>>(value);
            Assert.Single(employeeList);
            Assert.Equal("Alice", employeeList[0].Name);
        }
    }
}