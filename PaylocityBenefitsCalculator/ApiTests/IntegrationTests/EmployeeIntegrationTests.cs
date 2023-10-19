using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

[Collection("ApiTests")]
public class EmployeeIntegrationTests : IntegrationTest, IClassFixture<TestWebApplicationFactory>
{
    public EmployeeIntegrationTests(TestWebApplicationFactory applicationFactory)
        : base(applicationFactory)
    {
    }

    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var httpClient = ApplicationFactory.CreateClient();
        var response = await httpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
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
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }
    
    [Fact]
    //task: make test pass
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedToAddNewEmployee_ShouldReturnNewEmployee()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var newEmployee = new CreateEmployeeDto()
        {
            FirstName = "Magic",
            LastName = "Johnson",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1959, 8, 14),
        };
        var response = await httpClient.PostAsJsonAsync("/api/v1/employees", newEmployee);
        
        var expected = new GetEmployeeDto()
        {
            Id = 4,
            FirstName = newEmployee.FirstName,
            LastName = newEmployee.LastName,
            Salary = newEmployee.Salary,
            DateOfBirth = newEmployee.DateOfBirth
        };
        await response.ShouldReturn(HttpStatusCode.Created, expected);
    }

    [Fact]
    public async Task WhenAskedToUpdateEmployee_ShouldReturnUpdatedEmployee()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var newEmployeeData = new UpdateEmployeeDto()
        {
            FirstName = "LeBron2",
            LastName = "James2",
            Salary = 75420.90m,
            DateOfBirth = new DateTime(1984, 12, 31)
        };
        var response = await httpClient.PutAsJsonAsync("/api/v1/employees/1", newEmployeeData);

        var expected = new GetEmployeeDto()
        {
            Id = 1,
            FirstName = newEmployeeData.FirstName,
            LastName = newEmployeeData.LastName,
            Salary = newEmployeeData.Salary,
            DateOfBirth = newEmployeeData.DateOfBirth
        };
        await response.ShouldReturn(HttpStatusCode.OK, expected);
    }

    [Fact]
    public async Task WhenAskedToUpdateNonExistingEmployee_ShouldReturn404()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/employees/{int.MinValue}", new UpdateEmployeeDto());

        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedToDeleteEmployee_ShouldReturnNoContentAndEmployeeShouldNotExist()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.DeleteAsync("/api/v1/employees/1");

        await response.ShouldReturn(HttpStatusCode.NoContent);

        var response2 = await httpClient.GetAsync("/api/v1/employees/1");

        await response2.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedToDeleteNonExistingEmployee_ShouldReturn404()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.DeleteAsync($"/api/v1/employees/{int.MinValue}");

        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedForDependentsOfAnEmployee_ShouldReturnCorrectDependents()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.GetAsync("/api/v1/employees/2/dependents");
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
            }
        };
        
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]
    public async Task WhenAskedForPaychecksOfAnEmployee_ShouldReturnCorrectPaychecks()
    {
        var httpClient = ApplicationFactory.CreateClient();

        var response = await httpClient.GetAsync("/api/v1/employees/1/paychecks");
        var paychecks = new List<GetPaycheckDto>(27);
        paychecks.Add(new GetPaycheckDto
        {
            Id = 1,
            EmployeeId = 1,
            Year = 2022,
            Salary = 75420.99m,
            Deductions = 500
        });
        for (int i = 0;  i < 26; i++)
        {
            paychecks.Add(new GetPaycheckDto
            {
                Id = i + 2,
                EmployeeId = 1,
                Year = DateTime.Now.Year,
                Salary = 75420.99m,
                Deductions = i == 3 || i == 9 || i == 16 || i == 22 ? 461.53m : 461.54m
            });
        }
        
        await response.ShouldReturn(HttpStatusCode.OK, paychecks);
    }
}

