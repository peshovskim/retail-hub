namespace Cart.Application.Cart.Responses;

public sealed record CartResponse(
    Guid Id,
    string? AnonymousKey,
    IReadOnlyList<CartLineResponse> Lines,
    decimal Subtotal,
    int ItemCount);
