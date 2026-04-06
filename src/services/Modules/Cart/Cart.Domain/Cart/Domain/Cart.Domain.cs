using RetailHub.SharedKernel.Domain;

namespace Cart.Domain.Cart.Domain;

public sealed partial class Cart
{
    private const int MaxDistinctItems = 200;

    public Result AddOrUpdateItem(
        int productId,
        int quantityToAdd,
        decimal unitPrice,
        DateTime utcNow)
    {
        if (quantityToAdd <= 0)
        {
            return Result.Invalid(
                ResultCodes.Validation,
                "Quantity must be greater than zero.");
        }

        var existing = Items.FirstOrDefault(i => i.IsActive && i.ProductId == productId);

        if (existing is not null)
        {
            return existing.MergeQuantity(quantityToAdd, unitPrice, utcNow);
        }

        if (ActiveItemCount >= MaxDistinctItems)
        {
            return Result.Invalid(
                ResultCodes.Validation,
                $"Cart cannot contain more than {MaxDistinctItems} distinct products.");
        }

        var itemResult = CartItem.Create(Id, productId, quantityToAdd, unitPrice, utcNow);

        if (itemResult.IsFailure)
        {
            return Result.Failure(itemResult.Error!);
        }

        Items.Add(itemResult.Value!);

        return Result.Success();
    }

    public Result SetItemQuantity(int productId, int quantity, decimal unitPrice, DateTime utcNow)
    {
        var existing = Items.FirstOrDefault(i => i.IsActive && i.ProductId == productId);

        if (existing is null)
        {
            return Result.NotFound(ResultCodes.NotFound, "Product is not in the cart.");
        }

        if (quantity <= 0)
        {
            existing.SoftDelete(utcNow);

            return Result.Success();
        }

        return existing.SetQuantity(quantity, unitPrice, utcNow);
    }

    public void RemoveItem(int productId, DateTime utcNow)
    {
        var existing = Items.FirstOrDefault(i => i.IsActive && i.ProductId == productId);
        if (existing is not null)
        {
            existing.SoftDelete(utcNow);
        }
    }

    public void ClearAllActiveItems(DateTime utcNow)
    {
        foreach (var item in Items.Where(i => i.IsActive).ToList())
        {
            item.SoftDelete(utcNow);
        }
    }

    private int ActiveItemCount => Items.Count(i => i.IsActive);
}
