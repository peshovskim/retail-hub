using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class OrderLine
{
    internal static Result<OrderLine> Create(
        int productId,
        Guid productUid,
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

        decimal lineTotal = quantity * unitPrice;

        return Result<OrderLine>.Success(
            new OrderLine
            {
                Uid = Guid.NewGuid(),
                OrderId = 0,
                ProductId = productId,
                ProductUid = productUid,
                Quantity = quantity,
                UnitPrice = unitPrice,
                LineTotal = lineTotal,
                CreatedOn = createdOn,
                DeletedOn = null,
            });
    }
}
