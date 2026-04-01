using MediatR;
using RetailHub.BuildingBlocks.Application.Common.Abstractions.DomainEvents;
using RetailHub.BuildingBlocks.Domain;

namespace RetailHub.BuildingBlocks.Application.Common.DomainEvents;

public sealed class MediatRDomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(wrapperType, domainEvent)
                ?? throw new InvalidOperationException($"Could not wrap domain event type {domainEvent.GetType().Name}.");
            await publisher.Publish((INotification)notification, cancellationToken).ConfigureAwait(false);
        }
    }
}
