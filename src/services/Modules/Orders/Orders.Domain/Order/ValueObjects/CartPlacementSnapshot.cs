namespace Orders.Domain.Order.ValueObjects;

/// <summary>
/// Immutable snapshot of cart state needed to create an order aggregate,
/// without depending on the Cart bounded context.
/// </summary>
public sealed class CartPlacementSnapshot
{
    private readonly CartPlacementLineSnapshot[] _lines;

    public int CartId { get; }

    public Guid CartUid { get; }

    public DateTime? DeletedOn { get; }

    public IReadOnlyList<CartPlacementLineSnapshot> Lines => _lines;

    public CartPlacementSnapshot(int cartId, Guid cartUid, DateTime? deletedOn, CartPlacementLineSnapshot[] lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        CartId = cartId;
        CartUid = cartUid;
        DeletedOn = deletedOn;
        _lines = (CartPlacementLineSnapshot[])lines.Clone();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not CartPlacementSnapshot other)
        {
            return false;
        }

        if (CartId != other.CartId || CartUid != other.CartUid || DeletedOn != other.DeletedOn)
        {
            return false;
        }

        if (_lines.Length != other._lines.Length)
        {
            return false;
        }

        for (var i = 0; i < _lines.Length; i++)
        {
            if (_lines[i] != other._lines[i])
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(CartId);
        hash.Add(CartUid);
        hash.Add(DeletedOn);
        foreach (var line in _lines)
        {
            hash.Add(line);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(CartPlacementSnapshot? left, CartPlacementSnapshot? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(CartPlacementSnapshot? left, CartPlacementSnapshot? right)
        => !(left == right);
}
