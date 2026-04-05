using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.Application.User.Interfaces;

namespace RetailHub.Api.Services;

public sealed class HttpContextCurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUserSnapshot? GetSnapshot()
    {
        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
        {
            return null;
        }

        Claim? idClaim = identity.FindFirst("uid") ?? identity.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim is null || !Guid.TryParse(idClaim.Value, out Guid userId))
        {
            return null;
        }

        string? email = user.FindFirstValue(ClaimTypes.Email)
            ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);

        string[] roles = user
            .FindAll(ClaimTypes.Role)
            .Select(static c => c.Value)
            .ToArray();

        return new CurrentUserSnapshot(userId, email, roles);
    }
}
