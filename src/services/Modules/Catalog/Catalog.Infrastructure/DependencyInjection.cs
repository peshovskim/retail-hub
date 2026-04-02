using Catalog.Application.Category.Interfaces;
using Catalog.Infrastructure.Persistence.Read.Category.Factories;
using Catalog.Infrastructure.Persistence.Read.Category.Queries;
using Catalog.Infrastructure.Persistence.Write.Category.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Infrastructure;
using RetailHub.SharedKernel.Infrastructure.Persistence.Interceptors;

namespace Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDomainEventDispatchInterceptor<CatalogWriteDbContext>();
        services.AddDbContext<CatalogWriteDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(serviceProvider.GetRequiredService<DomainEventDispatchInterceptor<CatalogWriteDbContext>>());
        });
        services.AddDbContext<CatalogReadDbContext>(options =>
        {
            options.UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        services.AddScoped<CategoryReadFactory>();
        services.AddScoped<ICategoryReadRepository, CategoryQueries>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
