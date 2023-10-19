using Api.Models;

namespace Api.Interfaces
{
    public interface IBenefitCalculator
    {
        /// <summary>
        /// Calculate benefits costs for the specified employee and year.
        /// </summary>
        /// <param name="employee">The employee with dependents included.</param>
        /// <param name="year">The year to be calculated.</param>
        /// <returns>The list of amounts. Each amount represents the benefit cost for one paycheck
        ///     of the year.
        /// </returns>
        IList<decimal> Calculate(Employee employee, int year);
    }
}
