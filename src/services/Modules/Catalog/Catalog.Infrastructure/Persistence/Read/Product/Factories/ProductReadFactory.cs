using Catalog.Application.Product.Responses;
using ProductEntity = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure.Persistence.Read.Product.Factories;

public sealed class ProductReadFactory
{
    public ProductResponse ToResponse(
        ProductEntity product,
        string? categoryName = null,
        IReadOnlyList<ProductImageResponse>? images = null) =>
        new(
            product.Id,
            product.Uid,
            product.CategoryId,
            product.Name,
            product.Slug,
            product.Sku,
            product.Price,
            product.ShortDescription,
            product.Description,
            categoryName,
            images ?? Array.Empty<ProductImageResponse>());
}
