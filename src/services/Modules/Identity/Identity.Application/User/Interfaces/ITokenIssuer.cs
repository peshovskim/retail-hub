namespace Identity.Application.User.Interfaces;

public interface ITokenIssuer
{
    AccessTokenResult CreateAccessToken(Guid userId, string email, IEnumerable<string> roles);
}

public sealed record AccessTokenResult(string Token, DateTime ExpiresAtUtc);
