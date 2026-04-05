namespace Identity.Application.User.Responses;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    Guid UserId,
    string Email,
    IReadOnlyList<string> Roles);
