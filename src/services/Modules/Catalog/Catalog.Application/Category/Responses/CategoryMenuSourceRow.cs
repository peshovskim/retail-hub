using System.Text.Json.Serialization;

namespace Catalog.Application.Category.Responses;

public sealed record CategoryMenuSourceRow
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public int? ParentId { get; init; }

    [JsonConstructor]
    public CategoryMenuSourceRow(int id, string name, string slug, int? parentId)
    {
        Id = id;
        Name = name;
        Slug = slug;
        ParentId = parentId;
    }
}
