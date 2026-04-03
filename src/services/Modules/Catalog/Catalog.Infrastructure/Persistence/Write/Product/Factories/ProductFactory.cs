using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Persistence.Write.Product.Factories;

public static class ProductFactory
{
    public static ProductEntity Create(
        Guid id,
        DateTime createdOn,
        Guid categoryId,
        string name,
        string slug,
        string sku,
        decimal price,
        string shortDescription,
        string description) =>
        ProductEntity.Create(id, createdOn, categoryId, name, slug, sku, price, shortDescription, description);
}
