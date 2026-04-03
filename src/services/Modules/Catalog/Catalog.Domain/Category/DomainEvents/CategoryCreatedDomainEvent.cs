using RetailHub.SharedKernel.Domain;

namespace Catalog.Domain.Category.DomainEvents;

public sealed record CategoryCreatedDomainEvent : IDomainEvent
{
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public Guid? ParentId { get; init; }
    public DateTime CreatedOn { get; init; }

    public CategoryCreatedDomainEvent(Guid categoryId, string name, string slug, Guid? parentId, DateTime createdOn)
    {
        CategoryId = categoryId;
        Name = name;
        Slug = slug;
        ParentId = parentId;
        CreatedOn = createdOn;
    }
}
