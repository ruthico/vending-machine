using VendingMachineApi.Domain.Entities;

namespace VendingMachineApi.Application.Interfaces;

public interface IVendingMachineService
{
    VendingMachine CreateMachine(string name, string? location, int maxShelves);
    Shelf AddShelf(string machineId, string productType, int capacity);
    void LoadInventory(string machineId, string shelfId, int quantity, string productType);
    void PurchaseProduct(string machineId, string shelfId, string productType, int quantity);
    VendingMachine? GetMachine(string id);
    IEnumerable<VendingMachine> GetAllMachines();
}
