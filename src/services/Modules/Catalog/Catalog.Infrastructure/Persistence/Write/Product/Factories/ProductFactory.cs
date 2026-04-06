using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Persistence.Write.Product.Factories;

public static class ProductFactory
{
    public static ProductEntity Create(
        DateTime createdOn,
        int categoryId,
        Guid categoryUid,
        string name,
        string slug,
        string sku,
        decimal price,
        string shortDescription,
        string description) =>
        ProductEntity.Create(createdOn, categoryId, categoryUid, name, slug, sku, price, shortDescription, description);
}
