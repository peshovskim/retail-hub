namespace Cart.Domain.Cart.Domain;

public sealed partial class CartItem
{
    private CartItem()
    {
    }

    internal static CartItem Create(
        Guid id,
        Guid cartId,
        Guid productId,
        int quantity,
        decimal unitPrice,
        DateTime createdOn)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(quantity, 0);

        return new CartItem
        {
            Id = id,
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            CreatedOn = createdOn,
            DeletedOn = null,
            UpdatedOn = null,
        };
    }
}
