using MediatR;
using RetailHub.SharedKernel.Application.Common.Results;

namespace RetailHub.SharedKernel.Application.Common.Cqrs;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface ICommand : IRequest<Result>
{
}
