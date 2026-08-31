using VendingMachineApi.Application.Interfaces;
using VendingMachineApi.Domain.Exceptions;

namespace VendingMachineApi.Infrastructure.Catalogs;

public class InMemoryProductTypeCatalog : IProductTypeCatalog
{
    private readonly HashSet<string> _types = new(StringComparer.OrdinalIgnoreCase)
    {
        "drinks",
        "snacks",
        "cans"
    };

    public IReadOnlyCollection<string> GetAll()
    {
        return _types.OrderBy(x => x).ToList().AsReadOnly();
    }

    public bool IsAllowed(string productType)
    {
        if (string.IsNullOrWhiteSpace(productType))
            return false;

        return _types.Contains(productType.Trim());
    }

    public string Normalize(string productType)
    {
        if (string.IsNullOrWhiteSpace(productType))
            throw new DomainException("Product type must be provided.");

        return productType.Trim().ToLowerInvariant();
    }

    public void Add(string productType)
    {
        var normalized = Normalize(productType);
        if (_types.Contains(normalized))
            throw new DomainException("Product type already exists.");

        _types.Add(normalized);
    }
}
