namespace Cart.Application.Cart.Requests;

public sealed record UpdateCartItemRequest(Guid CartId, int Quantity);
