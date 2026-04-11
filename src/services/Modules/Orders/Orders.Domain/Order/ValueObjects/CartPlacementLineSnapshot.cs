namespace Orders.Domain.Order.ValueObjects;

/// <summary>
/// Immutable line data supplied when placing an order (Orders view of cart lines).
/// </summary>
public sealed class CartPlacementLineSnapshot
{
    public int ProductId { get; }

    public int Quantity { get; }

    public decimal UnitPrice { get; }

    public bool IsActive { get; }

    public CartPlacementLineSnapshot(int productId, int quantity, decimal unitPrice, bool isActive)
    {
        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(productId));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice));
        }

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        IsActive = isActive;
    }

    public override bool Equals(object? obj)
    {
        return obj is CartPlacementLineSnapshot other
               && ProductId == other.ProductId
               && Quantity == other.Quantity
               && UnitPrice == other.UnitPrice
               && IsActive == other.IsActive;
    }

    public override int GetHashCode()
        => HashCode.Combine(ProductId, Quantity, UnitPrice, IsActive);

    public static bool operator ==(CartPlacementLineSnapshot? left, CartPlacementLineSnapshot? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(CartPlacementLineSnapshot? left, CartPlacementLineSnapshot? right)
        => !(left == right);
}
