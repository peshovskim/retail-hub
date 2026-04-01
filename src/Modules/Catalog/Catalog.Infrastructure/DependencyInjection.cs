using Catalog.Application.Category.Interfaces;
using Catalog.Infrastructure.Read.Category.Factories;
using Catalog.Infrastructure.Read.Category.Queries;
using Catalog.Infrastructure.Write.Category.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Infrastructure;
using RetailHub.SharedKernel.Infrastructure.Persistence.Interceptors;

namespace Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDomainEventDispatchInterceptor<CatalogDbContext>();
        services.AddDbContext<CatalogDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(serviceProvider.GetRequiredService<DomainEventDispatchInterceptor<CatalogDbContext>>());
        });
        services.AddScoped<CategoryReadFactory>();
        services.AddScoped<ICategoryReadRepository, CategoryQueries>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
