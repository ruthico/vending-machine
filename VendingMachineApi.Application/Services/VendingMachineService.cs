using VendingMachineApi.Application.Interfaces;
using VendingMachineApi.Domain.Entities;
using VendingMachineApi.Domain.Exceptions;
using VendingMachineApi.Domain.ValueObjects;

namespace VendingMachineApi.Application.Services;

public class VendingMachineService : IVendingMachineService
{
    private readonly IVendingMachineRepository _repository;
    private readonly IProductTypeCatalog _productTypeCatalog;

    public VendingMachineService(IVendingMachineRepository repository, IProductTypeCatalog productTypeCatalog)
    {
        _repository = repository;
        _productTypeCatalog = productTypeCatalog;
    }

    public VendingMachine CreateMachine(string name, string? location, int maxShelves)
    {
        var machine = new VendingMachine(name, location, maxShelves);
        _repository.Add(machine);
        return machine;
    }

    public Shelf AddShelf(string machineId, string productType, int capacity)
    {
        var machine = GetMachineOrThrow(machineId);
        var normalizedType = ValidateAndNormalizeProductType(productType);
        return machine.AddShelf(new ProductType(normalizedType), capacity);
    }

    public void LoadInventory(string machineId, string shelfId, int quantity, string productType)
    {
        var machine = GetMachineOrThrow(machineId);
        var normalizedType = ValidateAndNormalizeProductType(productType);
        machine.LoadInventory(shelfId, quantity, new ProductType(normalizedType));
    }

    public void PurchaseProduct(string machineId, string shelfId, string productType, int quantity)
    {
        var machine = GetMachineOrThrow(machineId);
        var normalizedType = ValidateAndNormalizeProductType(productType);
        machine.Purchase(shelfId, new ProductType(normalizedType), quantity);
    }

    public VendingMachine? GetMachine(string id) => _repository.Get(id);

    public IEnumerable<VendingMachine> GetAllMachines() => _repository.GetAll();

    private VendingMachine GetMachineOrThrow(string id)
    {
        var machine = _repository.Get(id);
        if (machine == null)
            throw new DomainException("Machine not found.");

        return machine;
    }

    private string ValidateAndNormalizeProductType(string productType)
    {
        var normalized = _productTypeCatalog.Normalize(productType);
        if (!_productTypeCatalog.IsAllowed(normalized))
            throw new DomainException("Product type is not supported. Allowed types: drinks, snacks, cans.");

        return normalized;
    }
}
