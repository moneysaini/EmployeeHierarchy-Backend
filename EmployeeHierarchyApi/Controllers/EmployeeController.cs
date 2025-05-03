using EmployeeHierarchy.Application.DTOs;
using EmployeeHierarchy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeHierarchyApi.Controllers;
public class EmployeeController : BaseController
{   
    public EmployeeController(IEmployeeService employeeService) : base(employeeService) // Call the base constructor
    {
       
    }
   
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<List<EmployeeDto>>> UploadFile([FromForm] IFormFile file)
    {       
        try
        {
            if (file == null || file.Length == 0)
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid file." }); // 400 Bad Request

            var hierarchy = await _employeeService.BuildHierarchyAsync(file);
            return StatusCode(StatusCodes.Status200OK, new { message = hierarchy }); // 200 OK
        }
        catch (Exception ex)
        {          
            return HandleError<List<EmployeeDto>>(ex);// 500 Internal Server Error
        }
    }   
}
