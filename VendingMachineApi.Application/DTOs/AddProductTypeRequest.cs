using System.ComponentModel.DataAnnotations;

namespace VendingMachineApi.Application.DTOs;

public class AddProductTypeRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = null!;
}
