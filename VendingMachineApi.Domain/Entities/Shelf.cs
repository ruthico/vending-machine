using VendingMachineApi.Domain.Exceptions;
using VendingMachineApi.Domain.ValueObjects;

namespace VendingMachineApi.Domain.Entities;

public class Shelf
{
    public string Id { get; } = Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
    public string ProductType { get; }
    public int Capacity { get; }
    public int CurrentQuantity { get; private set; }

    public Shelf(ProductType productType, int capacity)
    {
        if (capacity <= 0)
            throw new DomainException("Capacity must be greater than zero.");

        ProductType = productType.Value;
        Capacity = capacity;
        CurrentQuantity = 0;
    }

    public void Load(int quantity, ProductType productType)
    {
        if (!string.Equals(productType.Value, ProductType, StringComparison.OrdinalIgnoreCase))
            throw new DomainException("Cannot load a different product type on this shelf.");
        if (quantity <= 0)
            throw new DomainException("Quantity to load must be positive.");
        if (CurrentQuantity + quantity > Capacity)
            throw new DomainException("Loading would exceed shelf capacity.");

        CurrentQuantity += quantity;
    }

    public void Remove(int quantity, ProductType productType)
    {
        if (!string.Equals(productType.Value, ProductType, StringComparison.OrdinalIgnoreCase))
            throw new DomainException("Cannot purchase a different product type from this shelf.");
        if (quantity <= 0)
            throw new DomainException("Quantity to purchase must be positive.");
        if (CurrentQuantity < quantity)
            throw new DomainException("Out of stock. Insufficient inventory.");

        CurrentQuantity -= quantity;
    }
}
