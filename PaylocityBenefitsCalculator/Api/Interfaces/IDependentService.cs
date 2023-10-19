using Api.Models;

namespace Api.Interfaces
{
    public interface IDependentService
    {
        Task<List<Dependent>> GetAllAsync();

        Task<Dependent?> GetByIdAsync(int id);

        Task<List<Dependent>> GetByEmployeeIdAsync(int employeeId);

        Task<Dependent> AddAsync(Dependent dependent);

        Task<Dependent> UpdateAsync(Dependent dependent);

        Task DeleteAsync(Dependent dependent);
    }
}
