using Api.Models;

namespace Api.Interfaces
{
    public interface IPaycheckService
    {
        Task<List<Paycheck>> GetAllAsync();

        Task<Paycheck?> GetByIdAsync(int id);

        Task<List<Paycheck>> GetByEmployeeIdAsync(int employeeId);

        /// <summary>
        /// Calculates or recalculates all paychecks of the specified 
        /// in the specified year.
        /// </summary>
        /// <param name="employeeId">The <see cref="Employee"/> Id.</param>
        /// <param name="year">The year to calculate.</param>
        Task CalculateAsync(int employeeId, int year);
    }
}
