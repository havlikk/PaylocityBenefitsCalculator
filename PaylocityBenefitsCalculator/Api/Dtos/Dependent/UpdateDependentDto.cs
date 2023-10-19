using Api.Models;

namespace Api.Dtos.Dependent;

public class UpdateDependentDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
}
