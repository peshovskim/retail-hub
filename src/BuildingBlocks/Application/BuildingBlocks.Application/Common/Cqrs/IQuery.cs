using MediatR;
using RetailHub.BuildingBlocks.Application.Common.Results;

namespace RetailHub.BuildingBlocks.Application.Common.Cqrs;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
