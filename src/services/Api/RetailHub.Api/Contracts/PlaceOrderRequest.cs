namespace RetailHub.Api.Contracts;

public sealed record PlaceOrderRequest(Guid CartId, Guid? UserId = null);
