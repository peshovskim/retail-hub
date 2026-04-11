using System.Text.Json.Serialization;

namespace Catalog.Application.Product.Responses;

public sealed record ProductResponse(
    [property: JsonIgnore] int ProductId,
    Guid Id,
    int CategoryId,
    string Name,
    string Slug,
    string Sku,
    decimal Price,
    string ShortDescription,
    string Description,
    string? CategoryName,
    IReadOnlyList<ProductImageResponse> Images);
