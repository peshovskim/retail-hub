using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Order.Interfaces;
using Orders.Infrastructure.Persistence.Read.Order.Queries;
using Orders.Infrastructure.Persistence.Write.Order.Repository;
using RetailHub.SharedKernel.Infrastructure;
using RetailHub.SharedKernel.Infrastructure.Persistence.Interceptors;

namespace Orders.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDomainEventDispatchInterceptor<OrdersWriteDbContext>();
        services.AddDbContext<OrdersWriteDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(serviceProvider.GetRequiredService<DomainEventDispatchInterceptor<OrdersWriteDbContext>>());
        });

        services.AddDbContext<OrdersReadDbContext>(options =>
        {
            options.UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IOrderReadRepository, OrderQueries>();

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
