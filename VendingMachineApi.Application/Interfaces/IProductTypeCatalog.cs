namespace VendingMachineApi.Application.Interfaces;

public interface IProductTypeCatalog
{
    IReadOnlyCollection<string> GetAll();
    bool IsAllowed(string productType);
    string Normalize(string productType);
    void Add(string productType);
}
