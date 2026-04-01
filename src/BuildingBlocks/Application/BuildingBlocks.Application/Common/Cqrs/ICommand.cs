using MediatR;
using RetailHub.BuildingBlocks.Application.Common.Results;

namespace RetailHub.BuildingBlocks.Application.Common.Cqrs;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface ICommand : IRequest<Result>
{
}
