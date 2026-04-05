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

        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        Claim? idClaim = FindUserIdClaim(user);

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

    private static Claim? FindUserIdClaim(ClaimsPrincipal user)
    {
        Claim? direct = user.FindFirst("uid")
            ?? user.FindFirst(JwtRegisteredClaimNames.Sub)
            ?? user.FindFirst(ClaimTypes.NameIdentifier);

        if (direct is not null)
        {
            return direct;
        }

        foreach (Claim claim in user.Claims)
        {
            if (!Guid.TryParse(claim.Value, out _))
            {
                continue;
            }

            if (claim.Type.Equals("uid", StringComparison.OrdinalIgnoreCase)
                || claim.Type.Equals(JwtRegisteredClaimNames.Sub, StringComparison.OrdinalIgnoreCase)
                || claim.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))
            {
                return claim;
            }
        }

        return null;
    }
}
