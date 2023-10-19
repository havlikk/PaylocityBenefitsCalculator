﻿namespace Api.Dtos.Employee
{
    public class CreateEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
