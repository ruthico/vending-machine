using System.ComponentModel.DataAnnotations;

namespace VendingMachineApi.Application.DTOs;

public class LoadInventoryRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string ProductType { get; set; } = null!;

    [Range(1, 1000)]
    public int Quantity { get; set; }
}
