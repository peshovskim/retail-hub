using Catalog.Domain.Product.DomainEvents;
using MediatR;
using RetailHub.SharedKernel.Application.Common.DomainEvents;

namespace Catalog.Application.Product.DomainEventHandlers;

public sealed class ProductCreatedDomainEventHandler
    : INotificationHandler<DomainEventNotification<ProductCreatedDomainEvent>>
{
    public Task Handle(
        DomainEventNotification<ProductCreatedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification.DomainEvent);
        return Task.CompletedTask;
    }
}
