using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Application.Cart.Interfaces;

public interface ICartRepository
{
    Task<CartEntity?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Lookup by session key without tracking (command-side read before insert).</summary>
    Task<CartEntity?> GetByAnonymousKeyAsNoTrackingAsync(
        string anonymousKey,
        CancellationToken cancellationToken = default);

    Task AddAsync(CartEntity cart, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
