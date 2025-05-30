using Microsoft.AspNetCore.Mvc;
using ShepherdsPie.Data;
using AutoMapper;
using ShepherdsPie.Models.DTOs;
using AutoMapper.QueryableExtensions;

namespace ShepherdsPie.Controllers;

[ApiController]
[Route("api/[controller]s")]

public class EmployeeController : ControllerBase
{
    private readonly ShepherdsPieDbContext _db;
    public EmployeeController(ShepherdsPieDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetAll(IMapper mapper)
    {
        return Ok(_db.Employees.ProjectTo<EmployeeDTO>(mapper.ConfigurationProvider).ToList());
    }
}
