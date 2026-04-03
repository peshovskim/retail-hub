using RetailHub.SharedKernel.Domain;

namespace Catalog.Domain.Product.Domain;

public sealed partial class Product : AggregateRoot
{
    public Guid CategoryId { get; set; }

    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public string Sku { get; private set; } = null!;

    public decimal Price { get; set; } 

    public string ShortDescription { get; private set; } = null!;

    public string Description { get; private set; } = null!;
}
