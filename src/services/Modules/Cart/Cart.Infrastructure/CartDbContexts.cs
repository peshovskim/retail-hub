using Cart.Infrastructure.Configurations.Cart;
using Cart.Infrastructure.Configurations.CartItem;
using Microsoft.EntityFrameworkCore;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Infrastructure;

public sealed class CartWriteDbContext : DbContext
{
    public CartWriteDbContext(DbContextOptions<CartWriteDbContext> options)
        : base(options)
    {
    }

    public DbSet<CartEntity> Carts => Set<CartEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CartWriteConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemWriteConfiguration());
    }
}

public sealed class CartReadDbContext : DbContext
{
    public CartReadDbContext(DbContextOptions<CartReadDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<CartEntity> Carts => Set<CartEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CartReadConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemReadConfiguration());
    }
}
