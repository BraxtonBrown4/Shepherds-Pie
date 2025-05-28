namespace ShepherdsPie.Model;
using System.ComponentModel.DataAnnotations;

public class Order
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    public int? DelivererId { get; set; }
    public int? TableNumber { get; set; }
    public decimal? Tip { get; set; }
    public decimal TotalCost { get; set; }
    public string OrderTimne { get; set; }
}