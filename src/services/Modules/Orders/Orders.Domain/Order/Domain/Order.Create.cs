using CartAggregate = Cart.Domain.Cart.Domain.Cart;
using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class Order
{
    public static Result<Order> PlaceFromCart(
        CartAggregate cart,
        int? userId,
        Guid? userUid,
        IReadOnlyDictionary<int, Guid> productUidByProductId,
        string status,
        DateTime utcNow)
    {
        ArgumentNullException.ThrowIfNull(cart);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(productUidByProductId);

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
            if (!productUidByProductId.TryGetValue(item.ProductId, out Guid productUid))
            {
                return Result<Order>.Invalid(
                    ResultCodes.Validation,
                    "One or more cart products could not be resolved.");
            }

            var lineResult = OrderLine.Create(
                item.ProductId,
                productUid,
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
            Uid = Guid.NewGuid(),
            CreatedOn = utcNow,
            DeletedOn = null,
            UserId = userId,
            UserUid = userUid,
            Status = status,
            CartId = cart.Id,
            CartUid = cart.Uid,
            TotalAmount = total,
        };

        foreach (var line in lines)
        {
            order.Lines.Add(line);
        }

        return Result<Order>.Success(order);
    }
}
