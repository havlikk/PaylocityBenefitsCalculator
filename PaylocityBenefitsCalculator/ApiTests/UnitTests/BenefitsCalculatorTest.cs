using Api.Models;
using Api.Services;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApiTests.UnitTests
{
    public class BenefitsCalculatorTest
    {
        [Fact]
        public void Calculate_NoDependents()
        {
            var calculator = new BenefitsCalculator();

            var result = calculator.Calculate(
                new Employee
                {
                    Id = 1,
                    Salary = 50_000,
                    Dependents = new List<Dependent>(0),
                },
                2023);
            
            Check.That(result).IsNotNull();
            Check.That(result.Count).IsEqualTo(26);
            Check.That(result.Sum()).IsEqualTo(12 * 1_000);
            CheckDeductionsSpreading(result);
        }

        [Fact]
        public void Calculate_TwoDependents()
        {
            var calculator = new BenefitsCalculator();

            var result = calculator.Calculate(
                new Employee 
                {
                    Id = 1,
                    Salary = 50_000,
                    Dependents = new List<Dependent> {
                        new () { DateOfBirth = new DateTime(2020, 1, 1)},
                        new () { DateOfBirth = new DateTime(2021, 1, 1)}
                    }
                },
                2023);

            Check.That(result).IsNotNull();
            Check.That(result.Count).IsEqualTo(26);
            Check.That(result.Sum()).IsEqualTo(12 * (1_000 +  2 * 600));
            CheckDeductionsSpreading(result);
        }

        [Fact]
        public void Calculate_HighSalaried()
        {
            var calculator = new BenefitsCalculator();

            var result = calculator.Calculate(
                new Employee
                {
                    Id = 1,
                    Salary = 100_000,
                    Dependents = new List<Dependent>(0),
                },
                2023);

            Check.That(result).IsNotNull();
            Check.That(result.Count).IsEqualTo(26);
            Check.That(result.Sum()).IsEqualTo(12 * 1_000 + 0.02 * 100_000);
            CheckDeductionsSpreading(result);
        }

        [Fact]
        public void Calculate_DependentReached50InJuly()
        {
            var calculator = new BenefitsCalculator();

            var result = calculator.Calculate(
                new Employee
                {
                    Id = 1,
                    Salary = 50_000,
                    Dependents = new List<Dependent>
                    {
                        new () { DateOfBirth = new DateTime (1973, 7, 1)}
                    }
                },
                2023);

            Check.That(result).IsNotNull();
            Check.That(result.Count).IsEqualTo(26);
            Check.That(result.Sum()).IsEqualTo(12 * 1_000 + 6 * 600 + 6 * 800);
            CheckDeductionsSpreading(result);
        }

        private void CheckDeductionsSpreading(ICollection<decimal> deductions)
        {
            foreach (var deduction in deductions)
            {
                Check.That(Math.Abs(deduction - deductions.First())).IsLessOrEqualThan(0.01m);
            }
        }
    }
}
