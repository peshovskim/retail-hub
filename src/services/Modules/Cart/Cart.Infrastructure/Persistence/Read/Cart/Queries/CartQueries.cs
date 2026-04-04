using Cart.Application.Cart.Interfaces;
using Cart.Infrastructure;
using Cart.Infrastructure.Persistence.Read.Cart.Factories;
using Microsoft.EntityFrameworkCore;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Infrastructure.Persistence.Read.Cart.Queries;

internal sealed class CartQueries : ICartReadRepository
{
    private readonly CartReadDbContext _dbContext;
    private readonly CartReadFactory _readFactory;

    public CartQueries(CartReadDbContext dbContext, CartReadFactory readFactory)
    {
        _dbContext = dbContext;
        _readFactory = readFactory;
    }

    public async Task<CartEntity?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _readFactory
            .ActiveCartsWithActiveItems(_dbContext)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<CartEntity?> GetByAnonymousKeyAsync(string anonymousKey, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Carts
            .Where(c => c.DeletedOn == null && c.AnonymousKey == anonymousKey)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
