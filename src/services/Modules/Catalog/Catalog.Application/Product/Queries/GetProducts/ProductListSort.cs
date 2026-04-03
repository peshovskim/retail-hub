namespace Catalog.Application.Product.Queries.GetProducts;

/// <summary>Whitelisted list ordering (map API values here; do not bind raw property names).</summary>
public enum ProductListSort
{
    NameAsc = 0,
    NameDesc = 1,
    PriceAsc = 2,
    PriceDesc = 3,
}
