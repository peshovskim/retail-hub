namespace Cart.Domain.Cart.Domain;

public sealed partial class Cart
{
    public static Cart Create(Guid id, DateTime createdOn, Guid? userId, string? anonymousKey)
    {
        return new Cart
        {
            Id = id,
            CreatedOn = createdOn,
            DeletedOn = null,
            UserId = userId,
            AnonymousKey = anonymousKey,
        };
    }
}
