using RetailHub.SharedKernel.Domain;

namespace Cart.Domain.Cart.Domain;

public sealed partial class CartItem
{
    internal void SoftDelete(DateTime utcNow)
    {
        DeletedOn = utcNow;
    }

    internal Result MergeQuantity(int quantityToAdd, decimal unitPrice, DateTime utcNow)
    {
        if (quantityToAdd <= 0)
        {
            return Result.Invalid(
                ResultCodes.Validation,
                "Quantity must be greater than zero.");
        }

        if (!IsActive)
        {
            return Result.Invalid(
                ResultCodes.Validation,
                "Cannot modify a removed cart line.");
        }

        Quantity += quantityToAdd;
        UnitPrice = unitPrice;
        UpdatedOn = utcNow;

        return Result.Success();
    }

    internal Result SetQuantity(int quantity, decimal unitPrice, DateTime utcNow)
    {
        if (quantity <= 0)
        {
            return Result.Invalid(
                ResultCodes.Validation,
                "Quantity must be greater than zero.");
        }

        if (!IsActive)
        {
            return Result.Invalid(
                ResultCodes.Validation,
                "Cannot modify a removed cart line.");
        }

        Quantity = quantity;
        UnitPrice = unitPrice;
        UpdatedOn = utcNow;

        return Result.Success();
    }
}
