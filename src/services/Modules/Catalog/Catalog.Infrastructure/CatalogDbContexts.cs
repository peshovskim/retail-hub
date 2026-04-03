using Catalog.Infrastructure.Configurations.Category;
using Catalog.Infrastructure.Configurations.Product;
using Microsoft.EntityFrameworkCore;
using Category = Catalog.Domain.Category.Domain.Category;
using Product = Catalog.Domain.Product.Domain.Product;

namespace Catalog.Infrastructure;

public sealed class CatalogWriteDbContext : DbContext
{
    public CatalogWriteDbContext(DbContextOptions<CatalogWriteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryWriteConfiguration());
        modelBuilder.ApplyConfiguration(new ProductWriteConfiguration());
    }
}

public sealed class CatalogReadDbContext : DbContext
{
    public CatalogReadDbContext(DbContextOptions<CatalogReadDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryReadConfiguration());
        modelBuilder.ApplyConfiguration(new ProductReadConfiguration());
    }
}
