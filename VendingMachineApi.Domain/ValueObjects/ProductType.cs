using VendingMachineApi.Domain.Exceptions;

namespace VendingMachineApi.Domain.ValueObjects;

public sealed class ProductType : IEquatable<ProductType>
{
    public string Value { get; }

    public ProductType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Product type must be provided.");

        Value = value.Trim().ToLowerInvariant();
    }

    public bool Equals(ProductType? other)
    {
        if (other is null) return false;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => obj is ProductType other && Equals(other);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;
}
