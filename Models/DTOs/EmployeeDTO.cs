namespace ShepherdsPie.Models.DTOs;
using System.ComponentModel.DataAnnotations;

public class EmployeeDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}