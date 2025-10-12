using Company.Backend.Data;
using Company.Backend.Respositories.Interfaces;
using Company.Shared.DTOs;
using Company.Shared.Entities;
using Company.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Company.Backend.Helpers;

namespace Company.Backend.Respositories.Implementations;

public class EmployeesRepository : GenericRepository<Employee>, IEmployeesRepository
{
    private readonly DataContext _context;

    public EmployeesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Employee>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x =>
                x.FirstName.ToLower().Contains(pagination.Filter.ToLower()) ||
                x.LastName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<Employee>>
        {
            WasSuccess = true,
            Result = await queryable.ToListAsync()
        };
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