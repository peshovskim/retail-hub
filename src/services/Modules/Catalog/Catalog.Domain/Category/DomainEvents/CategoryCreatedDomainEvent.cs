using RetailHub.SharedKernel.Domain;

namespace Catalog.Domain.Category.DomainEvents;

public sealed record CategoryCreatedDomainEvent : IDomainEvent
{
    public Guid CategoryUid { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public DateTime CreatedOn { get; init; }

    public CategoryCreatedDomainEvent(Guid categoryUid, string name, string slug, DateTime createdOn)
    {
        CategoryUid = categoryUid;
        Name = name;
        Slug = slug;
        CreatedOn = createdOn;
    }
}
