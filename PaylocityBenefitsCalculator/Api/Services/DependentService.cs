using Api.Data;
using Api.Exceptions;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class DependentService : IDependentService
    {
        private readonly BenefitsContext _benefitsContext;
        private readonly IPaycheckService _paycheckService;

        public DependentService(BenefitsContext benefitsContext, IPaycheckService paycheckService)
        {
            _benefitsContext = benefitsContext;
            _paycheckService = paycheckService;
        }

        public async Task<List<Dependent>> GetAllAsync()
        {
            return await _benefitsContext.Dependents.ToListAsync();
        }

        public async Task<Dependent?> GetByIdAsync(int id)
        {
            return await _benefitsContext.Dependents.FindAsync(id);
        }

        public async Task<List<Dependent>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _benefitsContext.Dependents
                .Where(d => d.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<Dependent> AddAsync(Dependent dependent)
        {
            if (await _benefitsContext.Employees.FindAsync(dependent.EmployeeId) == null)
                throw new ValidationException("Employee not found.");

            if (dependent.Relationship == Relationship.Spouse
                || dependent.Relationship == Relationship.DomesticPartner) 
            {
                await CheckAnotherPartnerNotExists(dependent.EmployeeId);
            }

            _benefitsContext.Dependents.Add(dependent);

            await _paycheckService.CalculateAsync(dependent.EmployeeId, DateTime.Now.Year);

            await _benefitsContext.SaveChangesAsync();

            return dependent;
        }

        public async Task DeleteAsync(Dependent dependent)
        {
            _benefitsContext.Remove(dependent);

            await _paycheckService.CalculateAsync(dependent.EmployeeId, DateTime.Now.Year);

            await _benefitsContext.SaveChangesAsync();
        }
        
        public async Task<Dependent> UpdateAsync(Dependent dependent)
        {
            if (dependent.Relationship == Relationship.Spouse
                || dependent.Relationship == Relationship.DomesticPartner)
            {
                await CheckAnotherPartnerNotExists(dependent.EmployeeId, dependent.Id);
            }

            _benefitsContext.Dependents.Update(dependent);

            await _paycheckService.CalculateAsync(dependent.EmployeeId, DateTime.Now.Year);

            await _benefitsContext.SaveChangesAsync();

            return dependent;
        }

        private async Task CheckAnotherPartnerNotExists(int employeeId, int excludeDependentId = 0)
        {
            if (await _benefitsContext.Dependents
                .AnyAsync(d => d.EmployeeId == employeeId
                    && d.Id != excludeDependentId
                    && (d.Relationship == Relationship.Spouse || d.Relationship == Relationship.DomesticPartner)))
            {
                throw new ValidationException("An employee may only have 1 spouse or domestic partner.");
            }
        }
    }
}
