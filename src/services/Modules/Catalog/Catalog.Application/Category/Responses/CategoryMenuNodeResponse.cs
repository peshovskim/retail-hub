namespace Catalog.Application.Category.Responses;

public sealed record CategoryMenuNodeResponse(
    Guid Id,
    string Name,
    string Slug,
    IReadOnlyList<CategoryMenuNodeResponse> Children);
