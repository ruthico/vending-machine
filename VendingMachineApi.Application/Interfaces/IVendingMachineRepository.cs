using VendingMachineApi.Domain.Entities;

namespace VendingMachineApi.Application.Interfaces;

public interface IVendingMachineRepository
{
    void Add(VendingMachine machine);
    VendingMachine? Get(string id);
    IEnumerable<VendingMachine> GetAll();
}
