namespace ShepherdsPie.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class CreateOrderDTO
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int? DelivererId { get; set; }
    public int? TableNumber { get; set; }
    public decimal? Tip { get; set; }
    public decimal? TotalCost { get; set; }
    public string OrderTime { get; set; }
    [Required]
    public List<PizzaDTO> Pizza { get; set; } = new List<PizzaDTO>();
}