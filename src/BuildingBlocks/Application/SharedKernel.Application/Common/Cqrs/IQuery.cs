using MediatR;
using RetailHub.SharedKernel.Application.Common.Results;

namespace RetailHub.SharedKernel.Application.Common.Cqrs;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
