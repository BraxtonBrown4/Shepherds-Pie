namespace ShepherdsPie.Model;
using System.ComponentModel.DataAnnotations;

public class Sauce
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}