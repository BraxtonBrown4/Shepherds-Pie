namespace ShepherdsPie.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class ToppingDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
}