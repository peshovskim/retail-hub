namespace Cart.Application.Cart.Responses;

public sealed record CartLineResponse(
    Guid ProductId,
    string Name,
    string Sku,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
