namespace Cart.Domain.Cart.Domain;

public sealed partial class Cart
{
    public static Cart Create(DateTime createdOn, int? userId, string? anonymousKey)
    {
        return new Cart
        {
            Uid = Guid.NewGuid(),
            CreatedOn = createdOn,
            DeletedOn = null,
            UserId = userId,
            AnonymousKey = anonymousKey,
        };
    }
}
