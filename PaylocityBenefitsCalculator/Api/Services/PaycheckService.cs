using Api.Data;
using Api.Exceptions;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class PaycheckService : IPaycheckService
    {
        private readonly BenefitsContext _benefitsContext;
        private readonly IBenefitCalculator _benefitCalculator;

        public const int PaychecksPerYear = 26;

        public PaycheckService(BenefitsContext benefitsContext, IBenefitCalculator benefitCalculator)
        {
            _benefitsContext = benefitsContext;
            this._benefitCalculator = benefitCalculator;
        }

        public async Task<List<Paycheck>> GetAllAsync()
        {
            return await _benefitsContext.Paychecks.ToListAsync();
        }

        public async Task<List<Paycheck>> GetByEmployeeIdAsync(int employeeId)
        {
            var thisYear = DateTime.Now.Year;
            var thisYearPaychecks = await _benefitsContext.Paychecks.
                Where(p => p.EmployeeId == employeeId && p.Year == thisYear)
                .ToListAsync();

            if (thisYearPaychecks.Count != PaychecksPerYear)
            {
                await CalculateAsync(employeeId, thisYear);
            }

            return await _benefitsContext.Paychecks.
                Where(p => p.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<Paycheck?> GetByIdAsync(int id)
        {
            return await _benefitsContext.Paychecks.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task CalculateAsync(int employeeId, int year)
        {
            var employee = await _benefitsContext.Employees
                .Include(e => e.Dependents)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new ValidationException("Employee not found.");

            var benefitDeductions = _benefitCalculator.Calculate(employee, year);

            var paychecks = await _benefitsContext.Paychecks
                .Where(p => p.EmployeeId == employee.Id && p.Year == year)
                .ToListAsync();

            if (paychecks.Count != benefitDeductions.Count)
            {
                _benefitsContext.RemoveRange(paychecks);
                for (int i = 0; i < benefitDeductions.Count; i++)
                {
                    _benefitsContext.Add(new Paycheck
                    {
                        EmployeeId = employee.Id,
                        Year = year,
                        Salary = employee.Salary,
                        Deductions = benefitDeductions[i]
                    });
                 }
            }
            else
            {
                for (int i = 0; i < benefitDeductions.Count; i++)
                {
                    paychecks[i].Deductions = benefitDeductions[i];
                }
            }

            await _benefitsContext.SaveChangesAsync();
        }
    }
}
