using Company.Shared.Entities;

namespace Company.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckEmployeesAsync();
    }

    private async Task CheckEmployeesAsync()
    {
        if (!_context.Employees.Any())
        {
            _context.Employees.Add(new Employee { FirstName = "Juan", LastName = "Gomez", IsActive = true, HireDate = DateTime.Now.AddYears(-2), Salary = 1500000M });
            _context.Employees.Add(new Employee { FirstName = "Juana", LastName = "Perez", IsActive = true, HireDate = DateTime.Now.AddYears(-1), Salary = 2000000M });
            _context.Employees.Add(new Employee { FirstName = "Julian", LastName = "Lopez", IsActive = false, HireDate = DateTime.Now.AddYears(-5), Salary = 2500000M });
            _context.Employees.Add(new Employee { FirstName = "Ana Julia", LastName = "Martinez", IsActive = true, HireDate = DateTime.Now.AddYears(-3), Salary = 1800000M });
            _context.Employees.Add(new Employee { FirstName = "Carlos", LastName = "Juarez", IsActive = true, HireDate = DateTime.Now.AddYears(-4), Salary = 1200000M });
            _context.Employees.Add(new Employee { FirstName = "Luisa", LastName = "San Juan", IsActive = true, HireDate = DateTime.Now.AddYears(-6), Salary = 2200000M });
            _context.Employees.Add(new Employee { FirstName = "Pedro", LastName = "Ramirez", IsActive = true, HireDate = DateTime.Now.AddYears(-7), Salary = 1400000M });
            _context.Employees.Add(new Employee { FirstName = "Miguel", LastName = "Santos", IsActive = false, HireDate = DateTime.Now.AddYears(-8), Salary = 1300000M });
            _context.Employees.Add(new Employee { FirstName = "Laura", LastName = "Diaz", IsActive = true, HireDate = DateTime.Now.AddYears(-1).AddMonths(-2), Salary = 1600000M });
            _context.Employees.Add(new Employee { FirstName = "Andres", LastName = "Quintero", IsActive = true, HireDate = DateTime.Now.AddMonths(-6), Salary = 1700000M });
            await _context.SaveChangesAsync();
        }
    }
}