namespace ShepherdsPie.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class OrderDetailsDTO
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int? DelivererId { get; set; }
    public int? TableNumber { get; set; }
    public decimal? Tip { get; set; }
    public decimal TotalCost { get; set; }
    public string OrderTime { get; set; }
    public List<Pizza> Pizzas { get; set; }
}