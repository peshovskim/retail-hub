using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Catalog.Application.Product.Queries.GetProducts;

namespace Catalog.Infrastructure.Caching;

internal static class ProductListCacheKeyBuilder
{
    /// <summary>Deterministic hash suffix for <see cref="GetProductsQuery"/> (used after list version prefix).</summary>
    public static string ComputeCriteriaHashSuffix(GetProductsQuery criteria)
    {
        var search = string.IsNullOrWhiteSpace(criteria.Search) ? string.Empty : criteria.Search.Trim();
        var categoryPart = criteria.CategoryIds is { Count: > 0 } ids
            ? string.Join(',', ids.OrderBy(id => id))
            : string.Empty;
        var priceMin = criteria.PriceMin?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        var priceMax = criteria.PriceMax?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        var page = criteria.Page?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        var pageSize = criteria.PageSize?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;

        var canonical =
            $"{search}|{categoryPart}|{priceMin}|{priceMax}|{criteria.Sort}|{page}|{pageSize}";
        var bytes = Encoding.UTF8.GetBytes(canonical);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
