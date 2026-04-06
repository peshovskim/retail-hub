namespace Catalog.Application.Category.Responses;

public sealed record CategoryMenuNodeResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public IReadOnlyList<CategoryMenuNodeResponse> Children { get; init; } = null!;

    public CategoryMenuNodeResponse(int id, string name, string slug, IReadOnlyList<CategoryMenuNodeResponse> children)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Children = children;
    }
}
