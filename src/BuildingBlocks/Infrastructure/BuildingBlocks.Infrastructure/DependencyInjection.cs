using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.BuildingBlocks.Application.Common.Abstractions;
using RetailHub.BuildingBlocks.Infrastructure.Persistence;
using RetailHub.BuildingBlocks.Infrastructure.Persistence.Interceptors;

namespace RetailHub.BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers <see cref="IUnitOfWork"/> for the given EF Core <see cref="DbContext"/> type.
    /// </summary>
    public static IServiceCollection AddEfUnitOfWork<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();
        return services;
    }

    /// <summary>
    /// Registers a singleton <see cref="DomainEventDispatchInterceptor{TDbContext}"/> for use with <see cref="DbContextOptionsBuilder.AddInterceptors"/>.
    /// </summary>
    public static IServiceCollection AddDomainEventDispatchInterceptor<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddSingleton<DomainEventDispatchInterceptor<TDbContext>>();
        return services;
    }
}
