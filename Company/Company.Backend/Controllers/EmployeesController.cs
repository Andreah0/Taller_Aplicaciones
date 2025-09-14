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
    public EmployeesController(IGenericUnitOfWork<Employee> unitOfWork) : base(unitOfWork)
    {
    }
}