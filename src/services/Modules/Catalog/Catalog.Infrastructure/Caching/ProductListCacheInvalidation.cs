using System.Text;
using Catalog.Application.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Infrastructure.Caching;

internal sealed class ProductListCacheInvalidation : IProductListCacheInvalidation
{
    private readonly IDistributedCache _cache;

    public ProductListCacheInvalidation(IDistributedCache cache)
    {
        _cache = cache;
    }

    public Task NotifyProductCatalogMutatedAsync(CancellationToken cancellationToken = default)
    {
        var version = Guid.NewGuid().ToString("N");
        var bytes = Encoding.UTF8.GetBytes(version);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365),
        };

        return _cache.SetAsync(CatalogCacheKeys.ProductListVersion, bytes, options, cancellationToken);
    }
}
