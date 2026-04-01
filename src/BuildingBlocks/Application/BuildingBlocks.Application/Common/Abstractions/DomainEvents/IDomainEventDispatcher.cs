using RetailHub.BuildingBlocks.Domain;

namespace RetailHub.BuildingBlocks.Application.Common.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
