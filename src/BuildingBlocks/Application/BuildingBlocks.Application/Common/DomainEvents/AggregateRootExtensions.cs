using RetailHub.BuildingBlocks.Application.Common.Abstractions.DomainEvents;
using RetailHub.BuildingBlocks.Domain;

namespace RetailHub.BuildingBlocks.Application.Common.DomainEvents;

public static class AggregateRootExtensions
{
    /// <summary>
    /// Publishes all pending domain events via <see cref="IDomainEventDispatcher"/> then removes each from the aggregate.
    /// Call after persistence succeeds (for example after SaveChanges).
    /// </summary>
    public static async Task DispatchDomainEventsAsync(
        this AggregateRoot aggregate,
        IDomainEventDispatcher dispatcher,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(aggregate);
        ArgumentNullException.ThrowIfNull(dispatcher);
        var events = aggregate.DomainEvents.ToList();
        if (events.Count == 0)
        {
            return;
        }

        await dispatcher.DispatchAsync(events, cancellationToken).ConfigureAwait(false);
        foreach (var e in events)
        {
            aggregate.RemoveDomainEvent(e);
        }
    }
}
