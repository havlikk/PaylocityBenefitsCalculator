using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly BenefitsContext _benefitsContext;
        private readonly IPaycheckService _paycheckService;

        public EmployeeService(BenefitsContext benefitsContext, IPaycheckService paycheckService)
        {
            _benefitsContext = benefitsContext;
            _paycheckService = paycheckService;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _benefitsContext.Employees
                .Include(e => e.Dependents)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _benefitsContext.Employees
                .Include(e => e.Dependents)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await _benefitsContext.Employees.AddAsync(employee);
            await _benefitsContext.SaveChangesAsync();

            await _paycheckService.CalculateAsync(employee.Id, DateTime.Now.Year);

            await _benefitsContext.SaveChangesAsync();

            return employee;
        }

        public async Task DeleteAsync(Employee employee)
        {
            _benefitsContext.Remove(employee);

            await _benefitsContext.SaveChangesAsync();
        }
        
        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _benefitsContext.Employees.Update(employee);

            await _paycheckService.CalculateAsync(employee.Id, DateTime.Now.Year);

            await _benefitsContext.SaveChangesAsync();

            return employee;
        }
    }
}
