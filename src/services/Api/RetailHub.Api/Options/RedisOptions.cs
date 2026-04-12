namespace RetailHub.Api.Options;

public sealed class RedisOptions
{
    public const string SectionName = "Redis";

    /// <summary>When empty, Redis-backed distributed cache is not registered.</summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>Optional key prefix for shared Redis (e.g. multiple apps).</summary>
    public string InstanceName { get; set; } = "RetailHub:";
}
