using Orders.Domain.Order.ValueObjects;
using CartAggregate = Cart.Domain.Cart.Domain.Cart;

namespace Orders.Application.Order.Commands.PlaceOrder;

internal static class CartPlacementSnapshotMapper
{
    public static CartPlacementSnapshot FromCart(CartAggregate cart)
    {
        ArgumentNullException.ThrowIfNull(cart);
        CartPlacementLineSnapshot[] lines = cart.Items
            .Select(i => new CartPlacementLineSnapshot(
                i.ProductId,
                i.Quantity,
                i.UnitPrice,
                i.IsActive))
            .ToArray();

        return new CartPlacementSnapshot(cart.Id, cart.Uid, cart.DeletedOn, lines);
    }
}
