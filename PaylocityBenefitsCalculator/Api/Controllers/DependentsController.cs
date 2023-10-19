using Api.Dtos.Dependent;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentService.GetByIdAsync(id);

        if (dependent == null)
        {
            return NotFound();
        }

        var result = new ApiResponse<GetDependentDto>
        {
            Data = new GetDependentDto(dependent),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetAllAsync();

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents.Select(d => new GetDependentDto(d)).ToList(),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Add a new dependent")]
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Create([FromBody] CreateDependentDto dependent)
    {
        var newDependent = new Dependent
        {
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            DateOfBirth = dependent.DateOfBirth,
            EmployeeId = dependent.EmployeeId,
            Relationship = dependent.Relationship
        };

        newDependent = await _dependentService.AddAsync(newDependent);

        var result = new ApiResponse<GetDependentDto>
        {
            Data = new GetDependentDto(newDependent),
            Success = true
        };

        return CreatedAtAction(nameof(Get), new { id = newDependent.Id }, result);
    }

    [SwaggerOperation(Summary = "Update an existing dependent")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Update([FromRoute] int id, [FromBody] UpdateDependentDto dependent)
    {
        var existingDependent = await _dependentService.GetByIdAsync(id);
        if (existingDependent == null)
        {
            return NotFound();
        }

        existingDependent.FirstName = dependent.FirstName;
        existingDependent.LastName = dependent.LastName;
        existingDependent.DateOfBirth = dependent.DateOfBirth;
        existingDependent.Relationship = dependent.Relationship;

        var updatedDependent = await _dependentService.UpdateAsync(existingDependent);

        var result = new ApiResponse<GetDependentDto>
        {
            Data = new GetDependentDto(updatedDependent),
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Delete an dependent")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _dependentService.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        await _dependentService.DeleteAsync(employee);

        return NoContent();
    }
}
