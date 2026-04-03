namespace Catalog.Application.Product.Responses;

public sealed record ProductResponse
{
    public Guid Id { get; init; }
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string Sku { get; init; } = null!;
    public decimal Price { get; init; }

    public ProductResponse(Guid id, Guid categoryId, string name, string slug, string sku, decimal price)
    {
        Id = id;
        CategoryId = categoryId;
        Name = name;
        Slug = slug;
        Sku = sku;
        Price = price;
    }
}
