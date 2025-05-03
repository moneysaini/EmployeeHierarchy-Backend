using EmployeeHierarchy.Application.DTOs;
using EmployeeHierarchy.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace EmployeeHierarchyApi.Tests
{
    public class EmployeeServiceTests
    {

        private Mock<IEmployeeService> mockEmployeeService;
        public EmployeeServiceTests()
        {
            mockEmployeeService = new Mock<IEmployeeService>();

            var expectedHierarchy = new List<EmployeeDto>
            {
                new EmployeeDto
                {
                    EmployeeId = 1,
                    Name = "Alice",
                    Reports = new List<EmployeeDto>
                    {
                        new EmployeeDto { EmployeeId = 2, Name = "Bob", Reports = new() },
                        new EmployeeDto { EmployeeId = 3, Name = "Charlie", Reports = new() }
                    }
                }
            };

            mockEmployeeService
                .Setup(s => s.BuildHierarchyAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(expectedHierarchy);
        }
        [Fact]
        public async Task ProcessEmployeeCsvAsync_ValidCsv_ReturnsCorrectHierarchy()
        {
            // Arrange
            var csvContent = "Employee Id,Employee Name,Manager Id\n1,Alice,\n2,Bob,1\n3,Charlie,1";
            var bytes = Encoding.UTF8.GetBytes(csvContent);
            var stream = new MemoryStream(bytes);
            var formFile = new FormFile(stream, 0, bytes.Length, "file", "employees.csv");            

            // Act
            var result = await mockEmployeeService.Object.BuildHierarchyAsync(formFile);

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Alice");
            result[0].Reports.Should().HaveCount(2);
        }
    }
}
