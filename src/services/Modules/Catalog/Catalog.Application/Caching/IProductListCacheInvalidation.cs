namespace Catalog.Application.Caching;

/// <summary>
/// Notifies that product catalog data used for list queries may have changed; implementations should
/// invalidate or version product list cache entries.
/// </summary>
public interface IProductListCacheInvalidation
{
    Task NotifyProductCatalogMutatedAsync(CancellationToken cancellationToken = default);
}
