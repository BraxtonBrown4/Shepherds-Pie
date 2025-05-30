namespace ShepherdsPie.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class PizzaDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int OrderId { get; set; }
    public int SizeId { get; set; }
    [Required]
    public int CheeseId { get; set; }
    [Required]
    public int SauceId { get; set; }
    public decimal Price { get; set; }
}