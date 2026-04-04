using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Infrastructure.Persistence.Write.Cart.Factories;

public static class CartFactory
{
    public static CartEntity Create(Guid id, DateTime createdOn, Guid? userId, string? anonymousKey) =>
        CartEntity.Create(id, createdOn, userId, anonymousKey);
}
