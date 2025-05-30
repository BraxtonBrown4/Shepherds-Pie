using Microsoft.AspNetCore.Mvc;
using ShepherdsPie.Data;
using ShepherdsPie.Models;
using AutoMapper;
using ShepherdsPie.Models.DTOs;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ShepherdsPie.Controllers;

[ApiController]
[Route("api/[controller]s")]

public class OrderController : ControllerBase
{
    private readonly ShepherdsPieDbContext _db;
    public OrderController(ShepherdsPieDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetAll(IMapper mapper)
    {
        return Ok(_db.Orders.ProjectTo<OrderDTO>(mapper.ConfigurationProvider).ToList());
    }

    [HttpGet("{Id}")]
    public IActionResult GetOne(int Id, IMapper mapper)
    {
        Order order = _db.Orders.Include(o => o.Pizzas).FirstOrDefault(o => o.Id == Id);

        return Ok(mapper.Map<OrderDetailsDTO>(order));
    }
}
