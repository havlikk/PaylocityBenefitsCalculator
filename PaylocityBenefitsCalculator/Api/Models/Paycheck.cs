namespace Api.Models
{
  public class Paycheck
  {
    public int Id { get; set; }

    public int Year { get; set; }

    public decimal Salary { get; set; }

    public decimal Deductions { get; set; }

    public int EmployeeId { get; set; }

    public Employee? Employee { get; set; }
  }
}
