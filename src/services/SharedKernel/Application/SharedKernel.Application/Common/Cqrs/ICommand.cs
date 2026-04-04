using MediatR;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Application.Common.Cqrs;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

public interface ICommand : IRequest<Result>
{
}
