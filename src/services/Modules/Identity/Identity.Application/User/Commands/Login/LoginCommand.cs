using Identity.Application.User.Interfaces;
using Identity.Application.User.Requests;
using Identity.Application.User.Responses;
using Identity.Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Identity.Application.User.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<AuthResponse>
{
    public LoginCommand(LoginRequest request)
        : this(request.Email, request.Password)
    {
    }
}

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly ITokenIssuer _tokenIssuer;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        ITokenIssuer tokenIssuer,
        UserManager<ApplicationUser> userManager,
        ILogger<LoginCommandHandler> logger)
    {
        _tokenIssuer = tokenIssuer;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        string normalizedEmail = request.Email.Trim();

        ApplicationUser? user = await _userManager
            .FindByEmailAsync(normalizedEmail)
            .ConfigureAwait(false);

        if (user is null)
        {
            return Result<AuthResponse>.Unauthorized(
                ResultCodes.Unauthorized,
                "Invalid email or password.");
        }

        bool validPassword = await _userManager
            .CheckPasswordAsync(user, request.Password)
            .ConfigureAwait(false);

        if (!validPassword)
        {
            return Result<AuthResponse>.Unauthorized(
                ResultCodes.Unauthorized,
                "Invalid email or password.");
        }

        var roles = await _userManager
            .GetRolesAsync(user)
            .ConfigureAwait(false);

        AccessTokenResult token = _tokenIssuer.CreateAccessToken(
            user.Uid,
            user.Email ?? normalizedEmail,
            roles);

        _logger.LogInformation("User {UserUid} signed in", user.Uid);

        return Result<AuthResponse>.Success(
            new AuthResponse(
                token.Token,
                token.ExpiresAtUtc,
                user.Uid,
                user.Email ?? normalizedEmail,
                roles.ToArray()));
    }
}
