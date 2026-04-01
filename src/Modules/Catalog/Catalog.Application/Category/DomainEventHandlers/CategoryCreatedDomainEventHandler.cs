using Catalog.Domain.Category.DomainEvents;
using MediatR;
using RetailHub.BuildingBlocks.Application.Common.DomainEvents;

namespace Catalog.Application.Category.DomainEventHandlers;

public sealed class CategoryCreatedDomainEventHandler
    : INotificationHandler<DomainEventNotification<CategoryCreatedDomainEvent>>
{
    public Task Handle(
        DomainEventNotification<CategoryCreatedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification.DomainEvent);
        return Task.CompletedTask;
    }
}
