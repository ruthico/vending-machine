using Microsoft.Extensions.DependencyInjection;
using VendingMachineApi.Application.Interfaces;
using VendingMachineApi.Infrastructure.Catalogs;
using VendingMachineApi.Infrastructure.Persistence;

namespace VendingMachineApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IVendingMachineRepository, InMemoryVendingMachineRepository>();
        services.AddSingleton<IProductTypeCatalog, InMemoryProductTypeCatalog>();
        return services;
    }
}
