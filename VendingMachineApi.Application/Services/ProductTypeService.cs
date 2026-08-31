using VendingMachineApi.Application.Interfaces;

namespace VendingMachineApi.Application.Services;

public class ProductTypeService : IProductTypeService
{
    private readonly IProductTypeCatalog _catalog;

    public ProductTypeService(IProductTypeCatalog catalog)
    {
        _catalog = catalog;
    }

    public IReadOnlyCollection<string> GetAll()
    {
        return _catalog.GetAll();
    }

    public string Add(string name)
    {
        _catalog.Add(name);
        return _catalog.Normalize(name);
    }
}
