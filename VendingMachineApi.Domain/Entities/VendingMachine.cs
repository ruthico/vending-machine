using VendingMachineApi.Domain.Exceptions;
using VendingMachineApi.Domain.ValueObjects;

namespace VendingMachineApi.Domain.Entities;

public class VendingMachine
{
    public string Id { get; } = Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
    public string Name { get; }
    public string? Location { get; }
    public int MaxShelves { get; }

    private readonly List<Shelf> _shelves = new();
    public IReadOnlyList<Shelf> Shelves => _shelves.AsReadOnly();

    public VendingMachine(string name, string? location, int maxShelves)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Machine name must be provided.");
        if (maxShelves <= 0)
            throw new DomainException("Max shelves must be greater than zero.");

        Name = name;
        Location = location;
        MaxShelves = maxShelves;
    }

    public Shelf AddShelf(ProductType productType, int capacity)
    {
        if (_shelves.Count >= MaxShelves)
            throw new DomainException("Maximum number of shelves reached.");

        var shelf = new Shelf(productType, capacity);
        _shelves.Add(shelf);
        return shelf;
    }

    public Shelf GetShelf(string shelfId)
    {
        var shelf = _shelves.FirstOrDefault(s => s.Id == shelfId);
        if (shelf == null)
            throw new DomainException("Shelf not found.");

        return shelf;
    }

    public void LoadInventory(string shelfId, int quantity, ProductType productType)
    {
        var shelf = GetShelf(shelfId);
        shelf.Load(quantity, productType);
    }

    public void Purchase(string shelfId, ProductType productType, int quantity)
    {
        var shelf = GetShelf(shelfId);
        shelf.Remove(quantity, productType);
    }
}
