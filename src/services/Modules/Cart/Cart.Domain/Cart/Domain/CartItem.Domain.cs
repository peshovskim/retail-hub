namespace Cart.Domain.Cart.Domain;

public sealed partial class CartItem
{
    internal void SoftDelete(DateTime utcNow)
    {
        DeletedOn = utcNow;
    }

    internal void MergeQuantity(int quantityToAdd, decimal unitPrice, DateTime utcNow)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(quantityToAdd, 0);
        if (!IsActive)
        {
            throw new InvalidOperationException("Cannot modify a removed cart line.");
        }

        Quantity += quantityToAdd;
        UnitPrice = unitPrice;
        UpdatedOn = utcNow;
    }

    internal void SetQuantity(int quantity, decimal unitPrice, DateTime utcNow)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(quantity, 0);
        if (!IsActive)
        {
            throw new InvalidOperationException("Cannot modify a removed cart line.");
        }

        Quantity = quantity;
        UnitPrice = unitPrice;
        UpdatedOn = utcNow;
    }
}
