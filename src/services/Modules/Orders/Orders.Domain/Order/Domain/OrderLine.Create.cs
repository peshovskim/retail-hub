using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class OrderLine
{
    internal static Result<OrderLine> Create(
        Guid id,
        Guid orderId,
        Guid productId,
        int quantity,
        decimal unitPrice,
        DateTime createdOn)
    {
        if (quantity <= 0)
        {
            return Result<OrderLine>.Invalid(
                ResultCodes.Validation,
                "Quantity must be greater than zero.");
        }

        var lineTotal = quantity * unitPrice;

        return Result<OrderLine>.Success(
            new OrderLine
            {
                Id = id,
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                LineTotal = lineTotal,
                CreatedOn = createdOn,
                DeletedOn = null,
            });
    }
}
