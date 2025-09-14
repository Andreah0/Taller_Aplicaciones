using Company.Backend.Data;
using Company.Backend.UnitsOfWork.Interfaces;
using Company.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Controllers;

namespace Company.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : GenericController<Employee>
{
    private readonly IEmployeesUnitOfWork _employeesUnitOfWork;

    public EmployeesController(IGenericUnitOfWork<Employee> unitOfWork, IEmployeesUnitOfWork employeesUnitOfWork) : base(unitOfWork)
    {
        _employeesUnitOfWork = employeesUnitOfWork;
    }

    [HttpGet("SearchEmployee/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var action = await _employeesUnitOfWork.GetByNameAsync(name);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound();
    }
}