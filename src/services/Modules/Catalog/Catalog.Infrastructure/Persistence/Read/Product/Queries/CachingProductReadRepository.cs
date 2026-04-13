using System.Text;
using System.Text.Json;
using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Queries.GetProducts;
using Catalog.Application.Product.Responses;
using Catalog.Infrastructure.Caching;
using Catalog.Infrastructure.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.Infrastructure.Persistence.Read.Product.Queries;

/// <summary>
/// Caches <see cref="IProductReadRepository.ListActiveProductsAsync"/> via <see cref="IDistributedCache"/>.
/// <see cref="ProductResponse.ProductId"/> is not serialized (JsonIgnore); cached list items have default ProductId — OK for API JSON shape.
/// </summary>
internal sealed class CachingProductReadRepository : IProductReadRepository
{
    private const string InitialListVersion = "0";

    private readonly ProductQueries _inner;
    private readonly IDistributedCache _cache;
    private readonly CatalogCacheOptions _options;
    private readonly ILogger<CachingProductReadRepository> _logger;

    public CachingProductReadRepository(
        ProductQueries inner,
        IDistributedCache cache,
        IOptions<CatalogCacheOptions> options,
        ILogger<CachingProductReadRepository> logger)
    {
        _inner = inner;
        _cache = cache;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<ProductListResult> ListActiveProductsAsync(
        GetProductsQuery criteria,
        CancellationToken cancellationToken = default)
    {
        string version = await GetListVersionAsync(cancellationToken);
        string hashSuffix = ProductListCacheKeyBuilder.ComputeCriteriaHashSuffix(criteria);
        string cacheKey = $"{CatalogCacheKeys.ProductListPrefix}{version}:{hashSuffix}";

        byte[]? cached = await _cache.GetAsync(cacheKey, cancellationToken);
        if (cached is not null)
        {
            _logger.LogDebug("Cache hit for product list {KeySuffix}", hashSuffix);
            var dto = JsonSerializer.Deserialize<ProductListResultJson>(cached, CatalogCacheJson.Options);
            if (dto?.Items is not null)
            {
                return new ProductListResult(dto.Items, dto.TotalCount);
            }
        }

        _logger.LogDebug("Cache miss for product list {KeySuffix}", hashSuffix);
        ProductListResult fresh = await _inner.ListActiveProductsAsync(criteria, cancellationToken);
        ProductListResultJson payload = new(fresh.Items.ToList(), fresh.TotalCount);
        await _cache.SetAsync(
                cacheKey,
                JsonSerializer.SerializeToUtf8Bytes(payload, CatalogCacheJson.Options),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.ProductListTtlMinutes),
                },
                cancellationToken);

        return fresh;
    }

    private async Task<string> GetListVersionAsync(CancellationToken cancellationToken)
    {
        byte[]? versionBytes = await _cache
            .GetAsync(CatalogCacheKeys.ProductListVersion, cancellationToken);
        if (versionBytes is null || versionBytes.Length == 0)
        {
            return InitialListVersion;
        }

        return Encoding.UTF8.GetString(versionBytes);
    }

    public Task<ProductResponse?> GetActiveProductByUidAsync(Guid uid, CancellationToken cancellationToken = default) =>
        _inner.GetActiveProductByUidAsync(uid, cancellationToken);

    public Task<ProductResponse?> GetActiveProductByInternalIdAsync(
        int productId,
        CancellationToken cancellationToken = default) =>
        _inner.GetActiveProductByInternalIdAsync(productId, cancellationToken);

    public Task<IReadOnlyDictionary<int, Guid>> GetProductUidsByIdsAsync(
        IEnumerable<int> productIds,
        CancellationToken cancellationToken = default) =>
        _inner.GetProductUidsByIdsAsync(productIds, cancellationToken);

    /// <summary>DTO for reliable System.Text.Json round-trip (concrete list type).</summary>
    private sealed class ProductListResultJson
    {
        public ProductListResultJson()
        {
        }

        public ProductListResultJson(List<ProductResponse> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public List<ProductResponse>? Items { get; set; }

        public int TotalCount { get; set; }
    }
}
