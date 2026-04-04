using CartAggregate = Cart.Domain.Cart.Domain.Cart;
using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class Order
{
    public static Result<Order> PlaceFromCart(
        Guid orderId,
        CartAggregate cart,
        Guid? userId,
        string status,
        DateTime utcNow,
        Func<Guid> orderLineIdFactory)
    {
        ArgumentNullException.ThrowIfNull(cart);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(orderLineIdFactory);

        if (cart.DeletedOn is not null)
        {
            return Result<Order>.Invalid(
                ResultCodes.Validation,
                "Cannot place an order from a deleted cart.");
        }

        var activeItems = cart.Items.Where(i => i.IsActive).ToList();

        if (activeItems.Count == 0)
        {
            return Result<Order>.Invalid(
                ResultCodes.Validation,
                "Cart is empty.");
        }

        var lines = new List<OrderLine>();
        decimal total = 0;

        foreach (var item in activeItems)
        {
            var lineResult = OrderLine.Create(
                orderLineIdFactory(),
                orderId,
                item.ProductId,
                item.Quantity,
                item.UnitPrice,
                utcNow);

            if (lineResult.IsFailure)
            {
                return Result.FromError<Order>(lineResult);
            }

            var line = lineResult.Value!;
            total += line.LineTotal;
            lines.Add(line);
        }

        var order = new Order
        {
            Id = orderId,
            CreatedOn = utcNow,
            DeletedOn = null,
            UserId = userId,
            Status = status,
            CartId = cart.Id,
            TotalAmount = total,
        };

        foreach (var line in lines)
        {
            order.Lines.Add(line);
        }

        return Result<Order>.Success(order);
    }
}
