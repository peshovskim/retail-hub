namespace Catalog.Application.Product.Responses;

public sealed record ProductImageResponse(Guid Uid, int SortOrder, string ImageUrl);
