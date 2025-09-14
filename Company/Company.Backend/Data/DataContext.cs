using Company.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(builder =>
        {
            builder.Property(e => e.Salary)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.ToTable(t =>
                t.HasCheckConstraint("CHK_Employee_Salary_Min", "Salary >= 1000000"));
        });
    }
}