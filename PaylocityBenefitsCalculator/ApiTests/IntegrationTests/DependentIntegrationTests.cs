using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

[Collection("ApiTests")]
public class DependentIntegrationTests : IntegrationTest, IClassFixture<TestWebApplicationFactory>
{
    public DependentIntegrationTests(TestWebApplicationFactory applicationFactory)
        : base(applicationFactory)
    { }
    

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var httpClient = ApplicationFactory.CreateClient();
        var response = await httpClient.GetAsync("/api/v1/dependents");
        var dependents = new List<GetDependentDto>
        {
            new()
            {
                Id = 1,
                FirstName = "Spouse",
                LastName = "Morant",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3)
            },
            new()
            {
                Id = 2,
                FirstName = "Child1",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            },
            new()
            {
                Id = 3,
                FirstName = "Child2",
                LastName = "Morant",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2)
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var httpClient = ApplicationFactory.CreateClient();
        var response = await httpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3)
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        var httpClient = ApplicationFactory.CreateClient();
        var response = await httpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedToAddNewDependent_ShouldReturnNewDependent()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var newDependent = new CreateDependentDto()
        {
            FirstName = "Mary",
            LastName = "James",
            DateOfBirth = new DateTime(2000, 8, 14),
            EmployeeId = 1,
            Relationship = Relationship.Child
        };
        var response = await httpClient.PostAsJsonAsync("/api/v1/dependents", newDependent);

        var expected = new GetDependentDto()
        {
            Id = 5,
            FirstName = newDependent.FirstName,
            LastName = newDependent.LastName,
            DateOfBirth = newDependent.DateOfBirth,
            Relationship = newDependent.Relationship
        };
        await response.ShouldReturn(HttpStatusCode.Created, expected);
    }

    [Fact]
    public async Task WhenAskedToAddSecondSpouse_ShouldReturn400()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var newDependent = new CreateDependentDto()
        {
            FirstName = "Spouce2",
            LastName = "Morant",
            DateOfBirth = new DateTime(2000, 8, 14),
            EmployeeId = 2,
            Relationship = Relationship.Spouse
        };
        var response = await httpClient.PostAsJsonAsync("/api/v1/dependents", newDependent);

        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task WhenAskedToUpdateDependent_ShouldReturnUpdatedDependent()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var newDependentData = new UpdateDependentDto()
        {
            FirstName = "Child",
            LastName = "Morant2",
            Relationship = Relationship.Child,
            DateOfBirth = new DateTime(2000, 3, 3)
        };
        var response = await httpClient.PutAsJsonAsync("/api/v1/dependents/1", newDependentData);

        var expected = new GetDependentDto()
        {
            Id = 1,
            FirstName = newDependentData.FirstName,
            LastName = newDependentData.LastName,
            Relationship = newDependentData.Relationship,
            DateOfBirth = newDependentData.DateOfBirth
        };
        await response.ShouldReturn(HttpStatusCode.OK, expected);
    }

    [Fact]
    public async Task WhenAskedToUpdateNonExistingEmployee_ShouldReturn404()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/dependents/{int.MinValue}", new UpdateEmployeeDto());

        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedToDeleteDependent_ShouldReturnNoContentAndDependentShouldNotExist()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.DeleteAsync("/api/v1/dependents/1");

        await response.ShouldReturn(HttpStatusCode.NoContent);

        var response2 = await httpClient.GetAsync("/api/v1/dependents/1");

        await response2.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedToDeleteNonExistingDependent_ShouldReturn404()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.DeleteAsync($"/api/v1/dependents/{int.MinValue}");

        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}

