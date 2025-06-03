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

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _db.Orders
            .Include(o => o.Pizza)
                .ThenInclude(p => p.Toppings)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        foreach (var pizza in order.Pizza)
        {
            _db.PizzaToppings.RemoveRange(pizza.Toppings);
        }

        _db.Pizzas.RemoveRange(order.Pizza);

        _db.Orders.Remove(order);

        _db.SaveChanges();

        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] OrderDTO orderDto, IMapper mapper)
    {
        Order order = _db.Orders
            .Include(o => o.Pizza)
                .ThenInclude(p => p.Toppings)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        order.EmployeeId = orderDto.EmployeeId;
        order.DelivererId = orderDto.DelivererId;
        order.TableNumber = orderDto.TableNumber;
        order.Tip = orderDto.Tip;
        order.TotalCost = orderDto.TotalCost;

        var existingPizzas = _db.Pizzas.Where(p => p.OrderId == id).ToList();
        var pizzaDtos = orderDto.Pizza;

        foreach (var existingPizza in existingPizzas)
        {
            if (!pizzaDtos.Any(p => p.Id == existingPizza.Id))
            {
                var toppingsToDelete = _db.PizzaToppings.Where(pt => pt.PizzaId == existingPizza.Id).ToList();
                _db.PizzaToppings.RemoveRange(toppingsToDelete);

                _db.Pizzas.Remove(existingPizza);
            }
        }

        foreach (var pizzaDto in pizzaDtos)
        {
            var existingPizza = existingPizzas.FirstOrDefault(p => p.Id == pizzaDto.Id);

            if (existingPizza != null)
            {
                mapper.Map(pizzaDto, existingPizza);

                var existingToppings = _db.PizzaToppings.Where(pt => pt.PizzaId == existingPizza.Id).ToList();
                var toppingDtos = pizzaDto.Toppings;

                foreach (var existingTopping in existingToppings)
                {
                    if (!toppingDtos.Any(t => t.Id == existingTopping.Id))
                    {
                        _db.PizzaToppings.Remove(existingTopping);
                    }
                }

                foreach (var toppingDto in toppingDtos)
                {
                    var existingTopping = existingToppings.FirstOrDefault(t => t.Id == toppingDto.Id);

                    if (existingTopping != null)
                    {
                        mapper.Map(toppingDto, existingTopping);
                    }
                    else
                    {
                        var newTopping = mapper.Map<PizzaTopping>(toppingDto);
                        newTopping.PizzaId = existingPizza.Id;
                        _db.PizzaToppings.Add(newTopping);
                    }
                }
            }
            else
            {
                var newPizza = mapper.Map<Pizza>(pizzaDto);
                newPizza.OrderId = id;
                _db.Pizzas.Add(newPizza);

                foreach (var toppingDto in pizzaDto.Toppings)
                {
                    var newTopping = mapper.Map<PizzaTopping>(toppingDto);
                    newTopping.PizzaId = newPizza.Id;
                    _db.PizzaToppings.Add(newTopping);
                }
            }
        }

        _db.SaveChanges();

        var updatedOrder = _db.Orders
            .Include(o => o.Pizza)
                .ThenInclude(p => p.Toppings)
                    .ThenInclude(pt => pt.Topping)
            .FirstOrDefault(o => o.Id == id);

        return Ok(mapper.Map<OrderDetailsDTO>(updatedOrder));
    }
}
