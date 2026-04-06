using Catalog.Application.Product.Interfaces;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Application.Cart.Responses;

internal static class CartResponseFactory
{
    public static async Task<CartResponse> CreateAsync(
        CartEntity cart,
        IProductReadRepository productReadRepository,
        CancellationToken cancellationToken)
    {
        var lines = new List<CartLineResponse>();
        decimal subtotal = 0;
        var itemCount = 0;

        foreach (var item in cart.Items.Where(static i => i.IsActive).OrderBy(static i => i.ProductId))
        {
            var product = await productReadRepository
                .GetActiveProductByInternalIdAsync(item.ProductId, cancellationToken)
                .ConfigureAwait(false);

            var name = product?.Name ?? "Unavailable";
            var sku = product?.Sku ?? string.Empty;
            var lineTotal = item.UnitPrice * item.Quantity;
            subtotal += lineTotal;
            itemCount += item.Quantity;
            lines.Add(new CartLineResponse(product?.Id ?? Guid.Empty, name, sku, item.Quantity, item.UnitPrice, lineTotal));
        }

        return new CartResponse(cart.Uid, cart.AnonymousKey, lines, subtotal, itemCount);
    }
}
