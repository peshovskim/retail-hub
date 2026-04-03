using MediatR;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Application.Common.DomainEvents;

/// <summary>
/// Wraps a domain event for MediatR publish (<see cref="INotification"/>).
/// </summary>
public sealed record DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; init; } = default!;

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}
