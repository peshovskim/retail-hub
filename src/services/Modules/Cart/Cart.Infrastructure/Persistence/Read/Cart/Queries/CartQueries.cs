using Cart.Application.Cart.Interfaces;
using Cart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Infrastructure.Persistence.Read.Cart.Queries;

internal sealed class CartQueries : ICartReadRepository
{
    private readonly CartReadDbContext _dbContext;

    public CartQueries(CartReadDbContext dbContext) => _dbContext = dbContext;

    public async Task<CartEntity?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await ActiveCartsWithActiveItems(_dbContext)
            .FirstOrDefaultAsync(c => c.Uid == id, cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<CartEntity> ActiveCartsWithActiveItems(CartReadDbContext db) =>
        db.Carts
            .Where(c => c.DeletedOn == null)
            .Include(c => c.Items.Where(i => i.DeletedOn == null));

    public async Task<CartEntity?> GetByAnonymousKeyAsync(string anonymousKey, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Carts
            .Where(c => c.DeletedOn == null && c.AnonymousKey == anonymousKey)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
