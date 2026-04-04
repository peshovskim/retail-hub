using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Application.Cart.Interfaces;

public interface ICartReadRepository
{
    Task<CartEntity?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CartEntity?> GetByAnonymousKeyAsync(string anonymousKey, CancellationToken cancellationToken = default);
}
