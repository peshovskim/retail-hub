using Cart.Application.Cart.Interfaces;
using Cart.Infrastructure.Persistence.Read.Cart.Factories;
using Cart.Infrastructure.Persistence.Read.Cart.Queries;
using Cart.Infrastructure.Persistence.Write.Cart.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Infrastructure;
using RetailHub.SharedKernel.Infrastructure.Persistence.Interceptors;

namespace Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCartInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDomainEventDispatchInterceptor<CartWriteDbContext>();
        services.AddDbContext<CartWriteDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(serviceProvider.GetRequiredService<DomainEventDispatchInterceptor<CartWriteDbContext>>());
        });

        services.AddDbContext<CartReadDbContext>(options =>
        {
            options.UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<CartReadFactory>();
        services.AddScoped<ICartReadRepository, CartQueries>();

        services.AddScoped<ICartRepository, CartRepository>();

        return services;
    }
}
