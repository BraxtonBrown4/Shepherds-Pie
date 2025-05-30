namespace ShepherdsPie.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class PizzaToppingDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ToppingId { get; set; }
    [Required]
    public int PizzaId { get; set; }
}