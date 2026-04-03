using Catalog.Application.Product.Responses;
using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Persistence.Read.Product.Factories;

public sealed class ProductReadFactory
{
    public ProductResponse ToResponse(ProductEntity product) =>
        new(product.Id, product.CategoryId, product.Name, product.Slug, product.Sku, product.Price);
}
