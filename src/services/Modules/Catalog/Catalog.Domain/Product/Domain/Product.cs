using RetailHub.SharedKernel.Domain;
using CategoryAggregate = Catalog.Domain.Category.Domain.Category;

namespace Catalog.Domain.Product.Domain;

public sealed partial class Product : AggregateRoot
{
    public int CategoryId { get; set; }

    /// <summary>EF / read-model navigation. Not required for <see cref="Create"/>.</summary>
    public CategoryAggregate? Category { get; set; }

    /// <summary>Product images (EF navigation).</summary>
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public string Sku { get; private set; } = null!;

    public decimal Price { get; set; }

    public string ShortDescription { get; private set; } = null!;

    public string Description { get; private set; } = null!;
}
