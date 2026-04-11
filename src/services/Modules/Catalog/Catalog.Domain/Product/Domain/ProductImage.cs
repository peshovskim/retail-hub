using RetailHub.SharedKernel.Domain;

namespace Catalog.Domain.Product.Domain;

/// <summary>Catalog product image row; URLs point at blob (or CDN) storage.</summary>
public sealed class ProductImage : Entity
{
    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public int SortOrder { get; set; }

    public string ImageUrl { get; set; } = null!;
}
