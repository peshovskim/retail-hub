namespace Catalog.Infrastructure.Options;

public sealed class CatalogCacheOptions
{
    public const string SectionName = "CatalogCache";

    public int CategoryMenuTtlMinutes { get; set; } = 15;
}
