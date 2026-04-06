using Catalog.Domain.Product.DomainEvents;

namespace Catalog.Domain.Product.Domain;

public sealed partial class Product
{
    private Product()
    {
    }

    public static Product Create(
        DateTime createdOn,
        int categoryId,
        Guid categoryUid,
        string name,
        string slug,
        string sku,
        decimal price,
        string shortDescription,
        string description)
    {
        var product = new Product
        {
            Uid = Guid.NewGuid(),
            CreatedOn = createdOn,
            CategoryId = categoryId,
            Name = name,
            Slug = slug,
            Sku = sku,
            Price = price,
            ShortDescription = shortDescription,
            Description = description,
            DeletedOn = null
        };

        product.AddDomainEvent(new ProductCreatedDomainEvent(
            product.Uid,
            categoryUid,
            name,
            slug,
            sku,
            price,
            shortDescription,
            description,
            createdOn));

        return product;
    }
}
