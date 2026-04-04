using Orders.Application.Order.Responses;

namespace Orders.Application.Order.Interfaces;

public interface IOrderReadRepository
{
    Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
