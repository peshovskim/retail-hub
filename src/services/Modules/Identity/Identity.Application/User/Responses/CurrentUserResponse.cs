using System.Text.Json.Serialization;

namespace Identity.Application.User.Responses;

public sealed record CurrentUserResponse(
    [property: JsonPropertyName("uid")] Guid UserId,
    string Email,
    IReadOnlyList<string> Roles);
