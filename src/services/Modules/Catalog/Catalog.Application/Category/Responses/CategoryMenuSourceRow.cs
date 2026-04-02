namespace Catalog.Application.Category.Responses;

public sealed record CategoryMenuSourceRow(Guid Id, string Name, string Slug, Guid? ParentId);
