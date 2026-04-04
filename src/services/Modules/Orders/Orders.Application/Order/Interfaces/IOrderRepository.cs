using OrderEntity = Orders.Domain.Order.Domain.Order;

namespace Orders.Application.Order.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(OrderEntity order, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
