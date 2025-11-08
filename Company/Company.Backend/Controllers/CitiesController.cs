using Company.Backend.UnitsOfWork.Interfaces;
using Company.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Company.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : GenericController<City>
{
    public CitiesController(IGenericUnitOfWork<City> unitOfWork) : base(unitOfWork)
    {
    }
}