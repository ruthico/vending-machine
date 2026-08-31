using System.ComponentModel.DataAnnotations;

namespace VendingMachineApi.Application.DTOs;

public class CreateMachineRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? Location { get; set; }

    [Required]
    [Range(1, 100)]
    public int? MaxShelves { get; set; }
}
