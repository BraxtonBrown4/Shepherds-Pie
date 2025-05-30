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
        Order order = _db.Orders.Include(o => o.Pizza).FirstOrDefault(o => o.Id == Id);

        return Ok(mapper.Map<OrderDetailsDTO>(order));
    }

    [HttpPost]
public IActionResult CreatePost([FromBody] CreateOrderDTO orderDto, IMapper mapper)
{
    int orderId = (_db.Orders.Any() ? _db.Orders.Max(o => o.Id) : 0) + 1;
    Order order = mapper.Map<Order>(orderDto);
    order.Id = orderId;
    order.OrderTime = DateTime.UtcNow.ToString("o");
    order.Pizza = new List<Pizza>();

    int nextPizzaId = (_db.Pizzas.Any() ? _db.Pizzas.Max(p => p.Id) : 0) + 1;
    int nextToppingId = (_db.PizzaToppings.Any() ? _db.PizzaToppings.Max(t => t.Id) : 0) + 1;

    foreach (var pizzaDto in orderDto.Pizza)
    {
        var pizza = mapper.Map<Pizza>(pizzaDto);
        pizza.Id = nextPizzaId++;
        pizza.OrderId = orderId;
        pizza.Toppings = new List<PizzaTopping>();

        foreach (var toppingDto in pizzaDto.Toppings)
        {
            var topping = mapper.Map<PizzaTopping>(toppingDto);
            topping.Id = nextToppingId++;
            topping.PizzaId = pizza.Id;
            pizza.Toppings.Add(topping);
        }

        order.Pizza.Add(pizza);
    }

    _db.Orders.Add(order);
    _db.SaveChanges();

    var displayOrder = _db.Orders
        .Include(o => o.Pizza)
            .ThenInclude(p => p.Toppings)
                .ThenInclude(pt => pt.Topping)
        .FirstOrDefault(o => o.Id == orderId);

    return Ok(mapper.Map<OrderDetailsDTO>(displayOrder));
}

}
