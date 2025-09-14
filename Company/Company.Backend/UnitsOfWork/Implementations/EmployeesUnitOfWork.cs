using Company.Backend.Repositories.Interfaces;
using Company.Backend.Respositories.Interfaces;
using Company.Backend.UnitsOfWork.Interfaces;
using Company.Shared.Entities;
using Company.Shared.Responses;

namespace Company.Backend.UnitsOfWork.Implementations;

public class EmployeesUnitOfWork : GenericUnitOfWork<Employee>, IEmployeesUnitOfWork
{
    private readonly IEmployeesRepository _repository;

    public EmployeesUnitOfWork(IEmployeesRepository repository) : base(repository)

    {
        _repository = repository;
    }

    public async Task<ActionResponse<List<Employee>>> GetByNameAsync(string name) => await _repository.GetByNameAsync(name);
}