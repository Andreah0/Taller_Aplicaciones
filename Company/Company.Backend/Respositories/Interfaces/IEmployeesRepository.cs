using Company.Backend.Repositories.Interfaces;
using Company.Shared.Entities;
using Company.Shared.Responses;

namespace Company.Backend.Respositories.Interfaces;

public interface IEmployeesRepository : IGenericRepository<Employee>
{
    Task<ActionResponse<List<Employee>>> GetByNameAsync(string name);
}