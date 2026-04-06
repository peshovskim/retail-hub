using RetailHub.SharedKernel.Domain;

namespace Cart.Domain.Cart.Domain;

public sealed partial class CartItem
{
    private CartItem()
    {
    }

    internal static Result<CartItem> Create(
        int cartId,
        int productId,
        int quantity,
        decimal unitPrice,
        DateTime createdOn)
    {
        if (quantity <= 0)
        {
            return Result<CartItem>.Invalid(
                ResultCodes.Validation,
                "Quantity must be greater than zero.");
        }

        return Result<CartItem>.Success(
            new CartItem
            {
                Uid = Guid.NewGuid(),
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                CreatedOn = createdOn,
                DeletedOn = null,
                UpdatedOn = null,
            });
    }
}
