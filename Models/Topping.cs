namespace ShepherdsPie.Model;
using System.ComponentModel.DataAnnotations;

public class Topping
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
}