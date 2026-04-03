using Catalog.Application.Product.Queries.GetProducts;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Application;

namespace Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogApplication(this IServiceCollection services)
    {
        services.AddSharedKernelValidators(typeof(GetProductsQuery).Assembly);
        return services;
    }
}
