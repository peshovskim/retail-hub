using Cart.Application.Cart.Interfaces;
using Cart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Infrastructure.Persistence.Write.Cart.Repository;

internal sealed class CartRepository : ICartRepository
{
    private readonly CartWriteDbContext _dbContext;

    public CartRepository(CartWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CartEntity?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task AddAsync(CartEntity cart, CancellationToken cancellationToken = default)
    {
        await _dbContext.Carts.AddAsync(cart, cancellationToken).ConfigureAwait(false);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}
