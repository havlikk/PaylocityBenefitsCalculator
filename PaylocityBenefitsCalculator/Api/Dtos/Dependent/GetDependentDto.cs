using Api.Models;

namespace Api.Dtos.Dependent;

public class GetDependentDto
{
    public GetDependentDto()
    {
    }

    public GetDependentDto(Api.Models.Dependent dependent)
    {
        Id = dependent.Id;
        DateOfBirth = dependent.DateOfBirth;
        FirstName = dependent.FirstName;
        LastName = dependent.LastName;
        Relationship = dependent.Relationship;
    }

    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
}
