namespace RetailHub.SharedKernel.Domain;

/// <summary>
/// Domain failure: <see cref="Type"/> drives HTTP mapping at the boundary; <see cref="Code"/> is a stable string for clients; <see cref="Message"/> is human-readable detail.
/// </summary>
public sealed record ResultError(ResultType Type, string Code, string Message);
