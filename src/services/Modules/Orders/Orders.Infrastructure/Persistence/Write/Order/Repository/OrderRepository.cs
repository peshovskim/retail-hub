using Orders.Application.Order.Interfaces;
using Orders.Infrastructure;
using OrderEntity = Orders.Domain.Order.Domain.Order;

namespace Orders.Infrastructure.Persistence.Write.Order.Repository;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly OrdersWriteDbContext _dbContext;

    public OrderRepository(OrdersWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}
