using Identity.Application.User.Interfaces;
using Identity.Application.User.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Identity.Application.User.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IQuery<CurrentUserResponse>;

public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserResponse>>
{
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetCurrentUserQueryHandler(ICurrentUserAccessor currentUserAccessor)
    {
        _currentUserAccessor = currentUserAccessor;
    }

    public Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        CurrentUserSnapshot? snapshot = _currentUserAccessor.GetSnapshot();

        if (snapshot is null)
        {
            return Task.FromResult(
                Result<CurrentUserResponse>.Unauthorized(
                    ResultCodes.Unauthorized,
                    "Authentication required."));
        }

        CurrentUserResponse response = new(
            snapshot.UserId,
            snapshot.Email ?? string.Empty,
            snapshot.Roles);

        return Task.FromResult(Result<CurrentUserResponse>.Success(response));
    }
}
