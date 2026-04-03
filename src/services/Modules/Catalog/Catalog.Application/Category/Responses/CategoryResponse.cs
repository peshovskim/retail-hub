namespace Catalog.Application.Category.Responses;

public sealed record CategoryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;

    public CategoryResponse(Guid id, string name, string slug)
    {
        Id = id;
        Name = name;
        Slug = slug;
    }
}
