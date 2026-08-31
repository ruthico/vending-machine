using System.ComponentModel.DataAnnotations;

namespace VendingMachineApi.Application.DTOs;

public class AddShelfRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string ProductType { get; set; } = null!;

    public int Capacity { get; set; }
}
