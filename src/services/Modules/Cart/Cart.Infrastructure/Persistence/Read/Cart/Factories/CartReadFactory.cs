using Cart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Infrastructure.Persistence.Read.Cart.Factories;

/// <summary>Read-side query composition for cart aggregates (same role as category/product read factories).</summary>
public sealed class CartReadFactory
{
    public IQueryable<CartEntity> ActiveCartsWithActiveItems(CartReadDbContext db)
    {
        return db.Carts
            .Where(c => c.DeletedOn == null)
            .Include(c => c.Items.Where(i => i.DeletedOn == null));
    }
}
