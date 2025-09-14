using Company.Backend.Data;
using Company.Backend.Respositories.Interfaces;
using Company.Shared.Entities;
using Company.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Company.Backend.Respositories.Implementations;

public class EmployeesRepository : GenericRepository<Employee>, IEmployeesRepository
{
    private readonly DataContext _context;

    public EmployeesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ActionResponse<List<Employee>>> GetByNameAsync(string name)
    {
        var employee = await _context.Employees.Where(x => x.FirstName.ToLower().Contains(name.ToLower()) || x.LastName.ToLower().Contains(name.ToLower())).ToListAsync();
        if (!employee.Any())
        {
            return new ActionResponse<List<Employee>>
            {
                Message = "Empleado no escontrado."
            };
        }
        return new ActionResponse<List<Employee>>
        {
            WasSuccess = true,
            Result = employee
        };
    }
}