using Identity.Application.User;
using Identity.Application.User.Interfaces;
using Identity.Application.User.Requests;
using Identity.Application.User.Responses;
using Identity.Infrastructure.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
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

    public RegisterUserCommandHandler(
        ITokenIssuer tokenIssuer,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _tokenIssuer = tokenIssuer;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        string normalizedEmail = request.Email.Trim();

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = normalizedEmail,
            Email = normalizedEmail,
            EmailConfirmed = true,
            CreatedOn = DateTime.UtcNow,
        };

        IdentityResult createResult = await _userManager
            .CreateAsync(user, request.Password)
            .ConfigureAwait(false);

        if (!createResult.Succeeded)
        {
            return MapIdentityFailure(createResult);
        }

        if (await _roleManager.RoleExistsAsync(UserRoles.Customer).ConfigureAwait(false))
        {
            IdentityResult roleResult = await _userManager
                .AddToRoleAsync(user, UserRoles.Customer)
                .ConfigureAwait(false);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user).ConfigureAwait(false);
                return Result<AuthResponse>.InternalError(
                    ResultCodes.InternalError,
                    string.Join(" ", roleResult.Errors.Select(static e => e.Description)));
            }
        }

        var roles = await _userManager
            .GetRolesAsync(user)
            .ConfigureAwait(false);

        AccessTokenResult token = _tokenIssuer.CreateAccessToken(user.Id, user.Email ?? normalizedEmail, roles);

        return Result<AuthResponse>.Success(
            new AuthResponse(
                token.Token,
                token.ExpiresAtUtc,
                user.Id,
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
