using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using RetailHub.SharedKernel.Application.Common.Abstractions.DomainEvents;
using RetailHub.SharedKernel.Application.Common.DomainEvents;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Infrastructure.Persistence.Interceptors;

/// <summary>
/// After successful SaveChanges, dispatches domain events on tracked <see cref="AggregateRoot"/> instances.
/// </summary>
public sealed class DomainEventDispatchInterceptor<TDbContext> : SaveChangesInterceptor
    where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatchInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not TDbContext db)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        List<AggregateRoot> aggregates = db.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .Distinct()
            .ToList();

        if (aggregates.Count == 0)
        {
            return result;
        }

        await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();
        IDomainEventDispatcher dispatcher = scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>();
        foreach (var aggregate in aggregates)
        {
            await aggregate.DispatchDomainEventsAsync(dispatcher, cancellationToken);
        }

        return result;
    }
}
