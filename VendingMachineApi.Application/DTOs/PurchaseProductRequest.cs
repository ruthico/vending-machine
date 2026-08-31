using System.ComponentModel.DataAnnotations;

namespace VendingMachineApi.Application.DTOs;

public class PurchaseProductRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string ProductType { get; set; } = null!;

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
