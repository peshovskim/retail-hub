using RetailHub.SharedKernel.Domain;
using ProductAggregate = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Domain.Category.Domain;
public sealed partial class Category : AggregateRoot
{
    public string Name { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public int? ParentId { get; private set; }

    /// <summary>Parent category when <see cref="ParentId"/> is set (EF navigation).</summary>
    public Category? Parent { get; set; }

    /// <summary>Child categories (EF navigation).</summary>
    public ICollection<Category> Children { get; set; } = new List<Category>();

    /// <summary>Products in this category (EF navigation).</summary>
    public ICollection<ProductAggregate> Products { get; set; } = new List<ProductAggregate>();
}
