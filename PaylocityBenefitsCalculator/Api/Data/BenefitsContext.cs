using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class BenefitsContext : DbContext
    {
        public BenefitsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Dependent> Dependents { get; set; } = null!;

        public DbSet<Paycheck> Paychecks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasKey(e => e.Id);

            modelBuilder.Entity<Dependent>().HasKey(d => d.Id);

            modelBuilder.Entity<Paycheck>().HasKey(p => p.Id);
        }
    }
}
