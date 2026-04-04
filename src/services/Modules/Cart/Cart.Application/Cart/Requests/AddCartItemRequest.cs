namespace Cart.Application.Cart.Requests;

public sealed record AddCartItemRequest(Guid CartId, Guid ProductId, int Quantity);
