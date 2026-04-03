namespace Catalog.Application.Product.Responses;

public sealed record ProductResponse(
    Guid Id,
    Guid CategoryId,
    string Name,
    string Slug,
    string Sku,
    decimal Price,
    string ShortDescription,
    string Description,
    string? CategoryName);
