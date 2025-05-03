using EmployeeHierarchy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EmployeeHierarchyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        protected readonly IEmployeeService _employeeService;       
        protected BaseController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        // Common properties or methods for all controllers
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        protected ActionResult<T> HandleError<T>(Exception ex)
        {            
            // Handle the error and return an appropriate response           
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message); // 500 Internal Server Error
        }
    }
}
