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
public class SauceController : ControllerBase
{
    private readonly ShepherdsPieDbContext _db;
    private readonly IMapper _mapper;

    public SauceController(ShepherdsPieDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sauces = await _db.Sauces
            .ProjectTo<SauceDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Ok(sauces);
    }
}