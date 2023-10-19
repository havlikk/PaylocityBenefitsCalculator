using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public class GetEmployeeDto
{
    public GetEmployeeDto()
    {
    }

    public GetEmployeeDto(Models.Employee employee)
    {
        Id = employee.Id;
        DateOfBirth = employee.DateOfBirth;
        FirstName = employee.FirstName;
        LastName = employee.LastName;
        Salary = employee.Salary;
        Dependents = employee.Dependents != null 
            ? employee.Dependents.Select(d => new GetDependentDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    DateOfBirth = d.DateOfBirth,
                    Relationship = d.Relationship
                }).ToList()
            : new List<GetDependentDto>();
    }

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime DateOfBirth { get; set; }
    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();
}
