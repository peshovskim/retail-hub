using MediatR;
using RetailHub.SharedKernel.Application.Common.Abstractions.DomainEvents;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Application.Common.DomainEvents;

public sealed class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;

    public MediatRDomainEventDispatcher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(wrapperType, domainEvent)
                ?? throw new InvalidOperationException($"Could not wrap domain event type {domainEvent.GetType().Name}.");
            await _publisher.Publish((INotification)notification, cancellationToken).ConfigureAwait(false);
        }
    }
}
