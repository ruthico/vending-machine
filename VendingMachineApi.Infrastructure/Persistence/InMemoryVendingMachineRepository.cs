using VendingMachineApi.Application.Interfaces;
using VendingMachineApi.Domain.Entities;

namespace VendingMachineApi.Infrastructure.Persistence;

public class InMemoryVendingMachineRepository : IVendingMachineRepository
{
    private readonly Dictionary<string, VendingMachine> _storage = new();

    public void Add(VendingMachine machine)
    {
        _storage[machine.Id] = machine;
    }

    public VendingMachine? Get(string id)
    {
        _storage.TryGetValue(id, out var machine);
        return machine;
    }

    public IEnumerable<VendingMachine> GetAll() => _storage.Values;
}
