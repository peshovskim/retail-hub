using Catalog.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure;

internal static class CatalogDbContextConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryReadConfiguration());
    }
}

public sealed class CatalogWriteDbContext(DbContextOptions<CatalogWriteDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        CatalogDbContextConfiguration.Configure(modelBuilder);
}

public sealed class CatalogReadDbContext(DbContextOptions<CatalogReadDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        CatalogDbContextConfiguration.Configure(modelBuilder);
}
