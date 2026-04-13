using Identity.Application.User;
using Identity.Application.User.Interfaces;
using Identity.Application.User.Requests;
using Identity.Application.User.Responses;
using Identity.Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Identity.Application.User.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password) : ICommand<AuthResponse>
{
    public RegisterUserCommand(RegisterUserRequest request)
        : this(request.Email, request.Password)
    {
    }
}

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponse>>
{
    private readonly ITokenIssuer _tokenIssuer;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        ITokenIssuer tokenIssuer,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _tokenIssuer = tokenIssuer;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        string normalizedEmail = request.Email.Trim();

        var user = new ApplicationUser
        {
            Uid = Guid.NewGuid(),
            UserName = normalizedEmail,
            Email = normalizedEmail,
            EmailConfirmed = true,
            CreatedOn = DateTime.UtcNow,
        };

        IdentityResult createResult = await _userManager
            .CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            return MapIdentityFailure(createResult);
        }

        if (await _roleManager.RoleExistsAsync(UserRoles.Customer))
        {
            IdentityResult roleResult = await _userManager
                .AddToRoleAsync(user, UserRoles.Customer);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return Result<AuthResponse>.InternalError(
                    ResultCodes.InternalError,
                    string.Join(" ", roleResult.Errors.Select(static e => e.Description)));
            }
        }

        IList<string> roles = await _userManager
            .GetRolesAsync(user);

        AccessTokenResult token = _tokenIssuer.CreateAccessToken(user.Uid, user.Email ?? normalizedEmail, roles);

        _logger.LogInformation("User {UserUid} registered", user.Uid);

        return Result<AuthResponse>.Success(
            new AuthResponse(
                token.Token,
                token.ExpiresAtUtc,
                user.Uid,
                user.Email ?? normalizedEmail,
                roles.ToArray()));
    }

    private static Result<AuthResponse> MapIdentityFailure(IdentityResult result)
    {
        if (result.Errors.Any(static e =>
                e.Code is "DuplicateEmail" or "DuplicateUserName"))
        {
            return Result<AuthResponse>.Conflicted(ResultCodes.Conflict, "A user with this email already exists.");
        }

        string message = string.Join(" ", result.Errors.Select(static e => e.Description));
        return Result<AuthResponse>.Invalid(ResultCodes.Validation, message);
    }
}
