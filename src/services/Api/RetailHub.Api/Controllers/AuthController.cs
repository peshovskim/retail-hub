using Identity.Application.User.Commands.Login;
using Identity.Application.User.Commands.RegisterUser;
using Identity.Application.User.Queries.GetCurrentUser;
using Identity.Application.User.Requests;
using Identity.Application.User.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ExtendedApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        Result<AuthResponse> result = await _mediator
            .Send(new RegisterUserCommand(request), cancellationToken)
            .ConfigureAwait(false);

        return OkOrError(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        Result<AuthResponse> result = await _mediator
            .Send(new LoginCommand(request), cancellationToken)
            .ConfigureAwait(false);

        return OkOrError(result);
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        Result<CurrentUserResponse> result = await _mediator
            .Send(new GetCurrentUserQuery(), cancellationToken)
            .ConfigureAwait(false);

        return OkOrError(result);
    }
}
