using RetailHub.BuildingBlocks.Domain;

namespace Catalog.Domain.Category.DomainEvents;

public sealed record CategoryCreatedDomainEvent(
    Guid CategoryId,
    string Name,
    string Slug,
    Guid? ParentId,
    DateTime CreatedOn) : IDomainEvent;
