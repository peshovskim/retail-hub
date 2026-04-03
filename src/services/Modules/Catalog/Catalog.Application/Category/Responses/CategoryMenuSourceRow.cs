namespace Catalog.Application.Category.Responses;

public sealed record CategoryMenuSourceRow
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public Guid? ParentId { get; init; }

    public CategoryMenuSourceRow(Guid id, string name, string slug, Guid? parentId)
    {
        Id = id;
        Name = name;
        Slug = slug;
        ParentId = parentId;
    }
}
