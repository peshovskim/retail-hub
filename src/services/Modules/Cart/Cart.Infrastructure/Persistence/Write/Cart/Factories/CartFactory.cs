using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace Cart.Infrastructure.Persistence.Write.Cart.Factories;

public static class CartFactory
{
    public static CartEntity Create(DateTime createdOn, int? userId, string? anonymousKey) =>
        CartEntity.Create(createdOn, userId, anonymousKey);
}
