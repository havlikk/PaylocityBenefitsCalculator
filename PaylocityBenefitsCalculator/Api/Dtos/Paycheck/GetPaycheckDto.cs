namespace Api.Dtos.Paycheck
{
    public class GetPaycheckDto
    {
        public GetPaycheckDto()
        {
        }

        public GetPaycheckDto(Models.Paycheck paycheck)
        {
            Id = paycheck.Id;
            EmployeeId = paycheck.EmployeeId;
            Year = paycheck.Year;
            Salary = paycheck.Salary;
            Deductions = paycheck.Deductions;
        }

        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int Year { get; set; }

        public decimal Salary { get; set; }

        public decimal Deductions { get; set; }
    }
}
