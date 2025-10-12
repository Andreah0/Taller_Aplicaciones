using Company.Shared.DTOs;
using Company.Shared.Entities;
using Company.Shared.Responses;

namespace Company.Backend.UnitsOfWork.Interfaces;

public interface IEmployeesUnitOfWork
{
    Task<ActionResponse<IEnumerable<Employee>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<List<Employee>>> GetByNameAsync(string name);
}