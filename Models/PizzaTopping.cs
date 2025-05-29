namespace ShepherdsPie.Models;
using System.ComponentModel.DataAnnotations;

public class PizzaTopping
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ToppingId { get; set; }
    [Required]
    public int PizzaId { get; set; }
}