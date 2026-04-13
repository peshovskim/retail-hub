using Orders.Domain.Order.ValueObjects;
using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class Order
{
    public static Result<Order> PlaceFromCartPlacement(
        CartPlacementSnapshot placement,
        int? userId,
        Guid? userUid,
        IReadOnlyDictionary<int, Guid> productUidByProductId,
        string status,
        DateTime utcNow)
    {
        ArgumentNullException.ThrowIfNull(placement);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(productUidByProductId);

        if (placement.DeletedOn is not null)
        {
            return Result<Order>.Invalid(
                ResultCodes.Validation,
                "Cannot place an order from a deleted cart.");
        }

        List<CartPlacementLineSnapshot> activeItems = placement.Lines.Where(i => i.IsActive).ToList();

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

            Result<OrderLine> lineResult = OrderLine.Create(
                item.ProductId,
                productUid,
                item.Quantity,
                item.UnitPrice,
                utcNow);

            if (lineResult.IsFailure)
            {
                return Result.FromError<Order>(lineResult);
            }

            OrderLine line = lineResult.Value!;
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
            CartId = placement.CartId,
            CartUid = placement.CartUid,
            TotalAmount = total,
        };

        foreach (var line in lines)
        {
            order.Lines.Add(line);
        }

        return Result<Order>.Success(order);
    }
}
