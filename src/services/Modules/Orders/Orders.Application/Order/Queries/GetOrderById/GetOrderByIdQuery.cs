using MediatR;
using Orders.Application.Order.Interfaces;
using Orders.Application.Order.Responses;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Orders.Application.Order.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderResponse>;

public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    private readonly IOrderReadRepository _orderReadRepository;

    public GetOrderByIdQueryHandler(IOrderReadRepository orderReadRepository)
    {
        _orderReadRepository = orderReadRepository;
    }

    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepository
            .GetByIdAsync(request.OrderId, cancellationToken)
            .ConfigureAwait(false);

        if (order is null)
        {
            return Result<OrderResponse>.NotFound(ResultCodes.NotFound, "Order not found.");
        }

        return Result<OrderResponse>.Success(order);
    }
}
