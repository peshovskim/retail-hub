namespace Cart.Application.Cart.Responses;

public sealed record CartSessionResponse(Guid CartId, string AnonymousKey);
