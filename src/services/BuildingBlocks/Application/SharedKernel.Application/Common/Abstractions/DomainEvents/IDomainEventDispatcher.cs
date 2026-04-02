using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Application.Common.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
