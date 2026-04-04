namespace Cart.Domain.Cart.Domain;

public sealed partial class Cart
{
    private const int MaxDistinctItems = 200;

    public void AddOrUpdateItem(Guid productId, int quantityToAdd, decimal unitPrice, DateTime utcNow, Func<Guid> idFactory)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(quantityToAdd, 0);

        var existing = Items.FirstOrDefault(i => i.IsActive && i.ProductId == productId);
        if (existing is not null)
        {
            existing.MergeQuantity(quantityToAdd, unitPrice, utcNow);
            return;
        }

        if (ActiveItemCount >= MaxDistinctItems)
        {
            throw new InvalidOperationException($"Cart cannot contain more than {MaxDistinctItems} distinct products.");
        }

        var item = CartItem.Create(idFactory(), Id, productId, quantityToAdd, unitPrice, utcNow);
        Items.Add(item);
    }

    public void SetItemQuantity(Guid productId, int quantity, decimal unitPrice, DateTime utcNow)
    {
        var existing = Items.FirstOrDefault(i => i.IsActive && i.ProductId == productId);
        if (existing is null)
        {
            throw new InvalidOperationException("Product is not in the cart.");
        }

        if (quantity <= 0)
        {
            existing.SoftDelete(utcNow);
            return;
        }

        existing.SetQuantity(quantity, unitPrice, utcNow);
    }

    public void RemoveItem(Guid productId, DateTime utcNow)
    {
        var existing = Items.FirstOrDefault(i => i.IsActive && i.ProductId == productId);
        if (existing is not null)
        {
            existing.SoftDelete(utcNow);
        }
    }

    private int ActiveItemCount => Items.Count(i => i.IsActive);
}
