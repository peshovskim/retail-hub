using System.Text.Json;
using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using Catalog.Infrastructure.Caching;
using Catalog.Infrastructure.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.Infrastructure.Persistence.Read.Category.Queries;

internal sealed class CachingCategoryReadRepository : ICategoryReadRepository
{
    private readonly CategoryQueries _inner;
    private readonly IDistributedCache _cache;
    private readonly CatalogCacheOptions _options;
    private readonly ILogger<CachingCategoryReadRepository> _logger;

    public CachingCategoryReadRepository(
        CategoryQueries inner,
        IDistributedCache cache,
        IOptions<CatalogCacheOptions> options,
        ILogger<CachingCategoryReadRepository> logger)
    {
        _inner = inner;
        _cache = cache;
        _options = options.Value;
        _logger = logger;
    }

    public Task<IReadOnlyList<CategoryResponse>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default) =>
        _inner.GetRootCategoriesAsync(cancellationToken);

    public async Task<IReadOnlyList<CategoryMenuSourceRow>> GetAllActiveCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        byte[]? cached = await _cache
            .GetAsync(CatalogCacheKeys.CategoryMenuSourceRows, cancellationToken)
            .ConfigureAwait(false);
        if (cached is not null)
        {
            _logger.LogDebug("Cache hit for {Key}", CatalogCacheKeys.CategoryMenuSourceRows);
            var list = JsonSerializer.Deserialize<List<CategoryMenuSourceRow>>(cached, CatalogCacheJson.Options);
            if (list is not null)
            {
                return list;
            }
        }

        _logger.LogDebug("Cache miss for {Key}", CatalogCacheKeys.CategoryMenuSourceRows);
        var fresh = await _inner.GetAllActiveCategoriesAsync(cancellationToken).ConfigureAwait(false);
        await _cache.SetAsync(
                CatalogCacheKeys.CategoryMenuSourceRows,
                JsonSerializer.SerializeToUtf8Bytes(fresh, CatalogCacheJson.Options),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CategoryMenuTtlMinutes),
                },
                cancellationToken)
            .ConfigureAwait(false);
        return fresh;
    }
}
