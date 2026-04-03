using RetailHub.SharedKernel.Domain;

namespace Catalog.Domain.Product.DomainEvents;

public sealed record ProductCreatedDomainEvent : IDomainEvent
{
    public Guid ProductId { get; init; }
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string Sku { get; init; } = null!;
    public decimal Price { get; init; }
    public string ShortDescription { get; init; } = null!;
    public string Description { get; init; } = null!;
    public DateTime CreatedOn { get; init; }

    public ProductCreatedDomainEvent(
        Guid productId,
        Guid categoryId,
        string name,
        string slug,
        string sku,
        decimal price,
        string shortDescription,
        string description,
        DateTime createdOn)
    {
        ProductId = productId;
        CategoryId = categoryId;
        Name = name;
        Slug = slug;
        Sku = sku;
        Price = price;
        ShortDescription = shortDescription;
        Description = description;
        CreatedOn = createdOn;
    }
}
