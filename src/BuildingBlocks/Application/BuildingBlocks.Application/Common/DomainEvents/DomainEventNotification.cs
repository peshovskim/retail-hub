using MediatR;
using RetailHub.BuildingBlocks.Domain;

namespace RetailHub.BuildingBlocks.Application.Common.DomainEvents;

/// <summary>
/// Wraps a domain event for MediatR publish (<see cref="INotification"/>).
/// </summary>
public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent) : INotification
    where TDomainEvent : IDomainEvent;
