using Microsoft.AspNetCore.Mvc;
using ShepherdsPie.Data;
using AutoMapper;
using ShepherdsPie.Models;
using ShepherdsPie.Models.DTOs;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ShepherdsPie.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class CheeseController : ControllerBase
{
    private readonly ShepherdsPieDbContext _db;
    private readonly IMapper _mapper;

    public CheeseController(ShepherdsPieDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cheeses = await _db.Cheeses
            .ProjectTo<CheeseDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(cheeses);
    }
}
