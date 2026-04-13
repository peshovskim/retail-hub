namespace Catalog.Infrastructure.Caching;

internal static class CatalogCacheKeys
{
    /// <summary>All active categories (flat rows) used to build the nav menu tree.</summary>
    public const string CategoryMenuSourceRows = "catalog:categories:menu";

    /// <summary>Version token for product list cache; bumping it invalidates all list entries without prefix delete.</summary>
    public const string ProductListVersion = "catalog:products:listVersion";

    /// <summary>Prefix for product list entries; full key is <c>catalog:products:list:{version}:{hash}</c>.</summary>
    public const string ProductListPrefix = "catalog:products:list:";
}
