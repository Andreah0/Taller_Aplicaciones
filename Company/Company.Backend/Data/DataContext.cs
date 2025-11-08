using Company.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Company.Backend.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
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