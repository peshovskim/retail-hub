namespace Catalog.Application.Product.Responses;

/// <summary>Filtered product list with total count (same filters, before paging).</summary>
public sealed record ProductListResult(IReadOnlyList<ProductResponse> Items, int TotalCount);
