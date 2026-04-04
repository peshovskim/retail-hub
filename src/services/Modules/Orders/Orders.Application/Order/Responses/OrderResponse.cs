namespace Orders.Application.Order.Responses;

public sealed record OrderResponse(
    Guid Id,
    Guid? UserId,
    Guid? CartId,
    string Status,
    decimal TotalAmount,
    IReadOnlyList<OrderLineResponse> Lines);
