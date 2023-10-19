using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IDependentService _dependentService;
    private readonly IPaycheckService _paycheckService;

    public EmployeesController(IEmployeeService employeeService, IDependentService dependentService,
        IPaycheckService paycheckService)
    {
        _employeeService = employeeService;
        _dependentService = dependentService;
        _paycheckService = paycheckService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = new GetEmployeeDto(employee),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees.Select(e => new GetEmployeeDto(e)).ToList(),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Add a new employee")]
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Create([FromBody] CreateEmployeeDto employee)
    {
        var newEmployee = new Employee
        {
            DateOfBirth = employee.DateOfBirth,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Salary = employee.Salary
        };

        newEmployee = await _employeeService.AddAsync(newEmployee);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = new GetEmployeeDto(newEmployee),
            Success = true
        };

        return CreatedAtAction(nameof(Get), new { id = newEmployee.Id }, result);
    }

    [SwaggerOperation(Summary = "Update an existing employee")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Update([FromRoute] int id, [FromBody] UpdateEmployeeDto employee)
    {
        var existingEmployee = await _employeeService.GetByIdAsync(id);
        if (existingEmployee == null)
        {
            return NotFound();
        }

        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.DateOfBirth = employee.DateOfBirth;
        existingEmployee.Salary = employee.Salary;

        var updatedEmployee = await _employeeService.UpdateAsync(existingEmployee);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = new GetEmployeeDto(updatedEmployee),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Delete an employee")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        await _employeeService.DeleteAsync(employee);

        return NoContent();
    }

    [SwaggerOperation(Summary = "Get dependents of the employee")]
    [HttpGet("{id}/Dependents")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetDependents([FromRoute] int id)
    {
        var dependents = await _dependentService.GetByEmployeeIdAsync(id);

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents.Select(d => new GetDependentDto(d)).ToList(),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get paychecks of the employee")]
    [HttpGet("{id}/Paychecks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<List<GetPaycheckDto>>>> GetPaychecks([FromRoute] int id)
    {
        var paychecks = await _paycheckService.GetByEmployeeIdAsync(id);

        var result = new ApiResponse<List<GetPaycheckDto>>
        {
            Data = paychecks.Select(p => new GetPaycheckDto(p)).ToList(),
            Success = true
        };

        return result;
    }
}
