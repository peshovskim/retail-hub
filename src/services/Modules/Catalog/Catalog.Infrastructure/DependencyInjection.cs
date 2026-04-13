using Catalog.Application.Caching;
using Catalog.Application.Category.Interfaces;
using Catalog.Application.Product.Interfaces;
using Catalog.Infrastructure.Persistence.Read.Category.Factories;
using Catalog.Infrastructure.Persistence.Read.Category.Queries;
using Catalog.Infrastructure.Caching;
using Catalog.Infrastructure.Persistence.Read.Product.Factories;
using Catalog.Infrastructure.Persistence.Read.Product.Queries;
using Catalog.Infrastructure.Persistence.Write.Category.Repository;
using Catalog.Infrastructure.Persistence.Write.Product.Repository;
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
        services.AddScoped<CategoryQueries>();
        services.AddScoped<ICategoryReadRepository, CachingCategoryReadRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<ProductReadFactory>();
        services.AddScoped<ProductQueries>();
        services.AddScoped<IProductReadRepository, CachingProductReadRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductListCacheInvalidation, ProductListCacheInvalidation>();

        return services;
    }
}
