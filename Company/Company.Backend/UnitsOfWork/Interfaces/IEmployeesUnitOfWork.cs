using Company.Shared.Entities;
using Company.Shared.Responses;

namespace Company.Backend.UnitsOfWork.Interfaces;

public interface IEmployeesUnitOfWork
{
    Task<ActionResponse<List<Employee>>> GetByNameAsync(string name);
}