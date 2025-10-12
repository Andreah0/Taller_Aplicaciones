using Company.Backend.Repositories.Interfaces;
using Company.Shared.DTOs;
using Company.Shared.Entities;
using Company.Shared.Responses;

namespace Company.Backend.Respositories.Interfaces;

public interface IEmployeesRepository : IGenericRepository<Employee>
{
    Task<ActionResponse<IEnumerable<Employee>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<List<Employee>>> GetByNameAsync(string name);
}