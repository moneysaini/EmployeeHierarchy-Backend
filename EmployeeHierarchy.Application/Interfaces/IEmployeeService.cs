using Microsoft.AspNetCore.Http;
using EmployeeHierarchy.Application.DTOs;

namespace EmployeeHierarchy.Application.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> BuildHierarchyAsync(IFormFile file);
}
