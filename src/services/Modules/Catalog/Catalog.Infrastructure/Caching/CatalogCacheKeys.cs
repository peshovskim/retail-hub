namespace Catalog.Infrastructure.Caching;

internal static class CatalogCacheKeys
{
    /// <summary>All active categories (flat rows) used to build the nav menu tree.</summary>
    public const string CategoryMenuSourceRows = "catalog:categories:menu";
}
