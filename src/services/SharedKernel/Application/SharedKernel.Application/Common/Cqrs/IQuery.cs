using MediatR;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.SharedKernel.Application.Common.Cqrs;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
