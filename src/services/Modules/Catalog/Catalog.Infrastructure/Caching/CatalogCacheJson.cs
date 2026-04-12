using System.Text.Json;

namespace Catalog.Infrastructure.Caching;

internal static class CatalogCacheJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };
}
