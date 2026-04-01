using RetailHub.BuildingBlocks.Domain;

namespace Catalog.Domain.Category.Domain;

public sealed partial class Category : AggregateRoot
{
    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public Guid? ParentId { get; private set; }
}
