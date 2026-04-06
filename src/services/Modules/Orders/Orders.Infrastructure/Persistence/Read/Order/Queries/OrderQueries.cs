using Microsoft.EntityFrameworkCore;
using Orders.Application.Order.Interfaces;
using Orders.Application.Order.Responses;
using Orders.Infrastructure;
using OrderEntity = Orders.Domain.Order.Domain.Order;

namespace Orders.Infrastructure.Persistence.Read.Order.Queries;

internal sealed class OrderQueries : IOrderReadRepository
{
    private readonly OrdersReadDbContext _dbContext;

    public OrderQueries(OrdersReadDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await ActiveOrdersWithActiveLines(_dbContext)
            .FirstOrDefaultAsync(o => o.Uid == id, cancellationToken)
            .ConfigureAwait(false);

        if (order is null)
        {
            return null;
        }

        var lines = order.Lines
            .Select(l => new OrderLineResponse(l.ProductUid, l.Quantity, l.UnitPrice, l.LineTotal))
            .ToList();

        return new OrderResponse(
            order.Uid,
            order.UserUid,
            order.CartUid,
            order.Status,
            order.TotalAmount,
            lines);
    }

    private static IQueryable<OrderEntity> ActiveOrdersWithActiveLines(OrdersReadDbContext db) =>
        db.Orders
            .Where(o => o.DeletedOn == null)
            .Include(o => o.Lines.Where(l => l.DeletedOn == null));
}
