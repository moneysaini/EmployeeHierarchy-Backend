using EmployeeHierarchy.Application.DTOs;
using EmployeeHierarchy.Application.Interfaces;
using EmployeeHierarchy.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace EmployeeHierarchy.Application.Services;

public class EmployeeService : IEmployeeService
{
    public async Task<List<EmployeeDto>> BuildHierarchyAsync(IFormFile file)
    {
        var employees = new Dictionary<int, Employee>();
        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);

        var header = await reader.ReadLineAsync(); // Skip header
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (line == null) continue;

            var parts = line.Split(',');

            var empId = int.Parse(parts[0]);
            var name = parts[1];
            var managerId = string.IsNullOrWhiteSpace(parts[2]) ? (int?)null : int.Parse(parts[2]);

            var employee = new Employee
            {
                EmployeeId = empId,
                Name = name,
                ManagerId = managerId
            };

            employees[empId] = employee;
        }

        foreach (var emp in employees.Values)
        {
            if (emp.ManagerId.HasValue && employees.ContainsKey(emp.ManagerId.Value))
            {
                employees[emp.ManagerId.Value].Reports.Add(emp);
            }
        }

        // Only return root employees (those without managers)
        return employees.Values
            .Where(e => e.ManagerId == null)
            .Select(ToDto)
            .ToList();
    }

    private static EmployeeDto ToDto(Employee emp) =>
        new()
        {
            EmployeeId = emp.EmployeeId,
            Name = emp.Name,
            Reports = emp.Reports.Select(ToDto).ToList()
        };
}
