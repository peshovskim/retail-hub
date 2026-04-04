using Microsoft.EntityFrameworkCore;
using Orders.Infrastructure.Configurations.Order;
using Orders.Infrastructure.Configurations.OrderLine;
using OrderEntity = Orders.Domain.Order.Domain.Order;

namespace Orders.Infrastructure;

public sealed class OrdersWriteDbContext : DbContext
{
    public OrdersWriteDbContext(DbContextOptions<OrdersWriteDbContext> options)
        : base(options)
    {
    }

    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderWriteConfiguration());
        modelBuilder.ApplyConfiguration(new OrderLineWriteConfiguration());
    }
}

public sealed class OrdersReadDbContext : DbContext
{
    public OrdersReadDbContext(DbContextOptions<OrdersReadDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderReadConfiguration());
        modelBuilder.ApplyConfiguration(new OrderLineReadConfiguration());
    }
}
