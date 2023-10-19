using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiTests;

public class IntegrationTest
{
    public IntegrationTest(TestWebApplicationFactory applicationFactory)
    {
        ApplicationFactory = applicationFactory;
        using (var scope = applicationFactory.Services.CreateScope())
        {
            var benefitsContext = scope.ServiceProvider.GetRequiredService<BenefitsContext>();

            // Clear and seed the database.
            benefitsContext.Database.EnsureDeleted();
            benefitsContext.Database.EnsureCreated();
            SeedAsync(benefitsContext).Wait();
        }
    }

    protected TestWebApplicationFactory ApplicationFactory { get; }

    private async Task SeedAsync(BenefitsContext benefitsContext)
    {
        if (!await benefitsContext.Employees.AnyAsync())
        {
            await benefitsContext.Employees.AddRangeAsync(new List<Employee>
            {
                new()
                {
                    FirstName = "LeBron",
                    LastName = "James",
                    Salary = 75420.99m,
                    DateOfBirth = new DateTime(1984, 12, 30),
                },
                new()
                {
                    FirstName = "Ja",
                    LastName = "Morant",
                    Salary = 92365.22m,
                    DateOfBirth = new DateTime(1999, 8, 10),
                    Dependents = new List<Dependent>
                    {
                        new()
                        {
                            FirstName = "Spouse",
                            LastName = "Morant",
                            Relationship = Relationship.Spouse,
                            DateOfBirth = new DateTime(1998, 3, 3)
                        },
                        new()
                        {
                            FirstName = "Child1",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2020, 6, 23)
                        },
                        new()
                        {
                            FirstName = "Child2",
                            LastName = "Morant",
                            Relationship = Relationship.Child,
                            DateOfBirth = new DateTime(2021, 5, 18)
                        }
                    }
                },
                new()
                {
                    FirstName = "Michael",
                    LastName = "Jordan",
                    Salary = 143211.12m,
                    DateOfBirth = new DateTime(1963, 2, 17),
                    Dependents = new List<Dependent>
                    {
                        new()
                        {
                            FirstName = "DP",
                            LastName = "Jordan",
                            Relationship = Relationship.DomesticPartner,
                            DateOfBirth = new DateTime(1974, 1, 2)
                        }
                    }
                }
            });

            await benefitsContext.SaveChangesAsync();

            await benefitsContext.Paychecks.AddAsync(new Paycheck
            {
                EmployeeId = 1,
                Year = 2022,
                Salary = 75420.99m,
                Deductions = 500
            });

            await benefitsContext.SaveChangesAsync();
        }
    }
}

