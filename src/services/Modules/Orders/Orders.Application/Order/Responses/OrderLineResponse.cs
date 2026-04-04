namespace Orders.Application.Order.Responses;

public sealed record OrderLineResponse(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
