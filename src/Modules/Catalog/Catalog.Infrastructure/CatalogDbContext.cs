using Catalog.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure;

public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryReadConfiguration());
    }
}
