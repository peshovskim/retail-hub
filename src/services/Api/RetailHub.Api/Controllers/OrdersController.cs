using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Order.Commands.PlaceOrder;
using Orders.Application.Order.Queries.GetOrderById;
using Orders.Application.Order.Responses;
using RetailHub.Api.Contracts;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ExtendedApiController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Creates an order from the current cart and clears cart line items.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        Result<OrderResponse> result = await _mediator
            .Send(new PlaceOrderCommand(request.CartId, request.UserId), cancellationToken);

        return OkOrError(result);
    }

    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrder(Guid orderId, CancellationToken cancellationToken)
    {
        Result<OrderResponse> result = await _mediator
            .Send(new GetOrderByIdQuery(orderId), cancellationToken);

        return OkOrError(result);
    }
}
