using Api.Interfaces;
using Api.Models;

namespace Api.Services;

public class BenefitsCalculator : IBenefitCalculator
{
    private const decimal BaseCost = 1_000;
    private const decimal DependentCost = 600;
    private const decimal HighSalaryLimit = 80_000;
    private const decimal HighSalariedExtraPerc = 2;
    private const int HighAgeLimit = 50;
    private const decimal HighAgeExtraCost = 200;

    /// <inheritdoc/>
    public IList<decimal> Calculate(Employee employee, int year)
    {
        decimal yearlyCosts = GetYearlyCosts(year, employee.Salary, employee.Dependents);

        var benefitCosts = new List<decimal>(PaycheckService.PaychecksPerYear);
        decimal costPerPaycheck = yearlyCosts / PaycheckService.PaychecksPerYear;

        // Spread yearly costs among paychecks with a maximum difference of 1 cent.
        decimal costsIncluded = 0;
        for (int i = 1; i <= PaycheckService.PaychecksPerYear; i++)
        {
            decimal benefitCost;
            if (i == PaycheckService.PaychecksPerYear)
            {
                benefitCost = yearlyCosts - costsIncluded;
            }
            else
            {
                benefitCost = Math.Round(i * costPerPaycheck - costsIncluded, 2, MidpointRounding.AwayFromZero);
            }

            benefitCosts.Add(benefitCost);

            costsIncluded += benefitCost;
        }

        return benefitCosts;
    }

    private decimal GetYearlyCosts(int year, decimal salary, ICollection<Dependent> dependents)
    {
        decimal yearlyCosts = 0;
        for (int month = 1; month <= 12; month++)
        {
            // Base cost per month.
            var monthlyCosts = BaseCost;
            
            var firstDayOfMonth = new DateTime(year, month, 1);

            // Base cost per dependents.
            monthlyCosts += DependentCost * dependents.Where(d => d.DateOfBirth <= firstDayOfMonth).Count();

            // Addition cost per dependends older the 50 years.
            // TODO: It is not clear if the age limit is 50 included.
            monthlyCosts += HighAgeExtraCost *
                dependents.Where(d => d.DateOfBirth.AddYears(HighAgeLimit) <= firstDayOfMonth).Count();

            yearlyCosts += monthlyCosts;
        }

        // Additional cost for highly salaried employees.
        if (salary > HighSalaryLimit)
        {
            // TODO: Rounding need to be discussed.
            yearlyCosts += Math.Round(salary * HighSalariedExtraPerc / 100, 2, MidpointRounding.AwayFromZero);
        }

        return yearlyCosts;
    }
}