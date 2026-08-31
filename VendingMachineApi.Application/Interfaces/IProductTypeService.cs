namespace VendingMachineApi.Application.Interfaces;

public interface IProductTypeService
{
    IReadOnlyCollection<string> GetAll();
    string Add(string name);
}
