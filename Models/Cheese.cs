namespace ShepherdsPie.Model;
using System.ComponentModel.DataAnnotations;

public class Cheese
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}