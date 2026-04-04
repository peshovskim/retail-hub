using Cart.Application.Cart.Commands.AddCartItem;
using Cart.Application.Cart.Commands.CreateOrGetCartSession;
using Cart.Application.Cart.Commands.RemoveCartItem;
using Cart.Application.Cart.Commands.UpdateCartItemQuantity;
using Cart.Application.Cart.Queries.GetCart;
using Cart.Application.Cart.Requests;
using Cart.Application.Cart.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RetailHub.Api.Common.Http;
using RetailHub.SharedKernel.Application.Common.Results;

namespace RetailHub.Api.Controllers;

[ApiController]
[Route("api/cart")]
public sealed class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{cartId:guid}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(Guid cartId, CancellationToken cancellationToken)
    {
        Result<CartResponse> result = await _mediator.Send(new GetCartQuery(cartId), cancellationToken).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpPost("items")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest request, CancellationToken cancellationToken)
    {
        Result<CartResponse> result = await _mediator
            .Send(new AddCartItemCommand(request), cancellationToken)
            .ConfigureAwait(false);

        return result.ToActionResult();
    }

    [HttpPatch("items/{productId:guid}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem(
        Guid productId,
        [FromBody] UpdateCartItemRequest request,
        CancellationToken cancellationToken)
    {
        Result<CartResponse> result = await _mediator
            .Send(new UpdateCartItemQuantityCommand(request, productId), cancellationToken)
            .ConfigureAwait(false);
        return result.ToActionResult();
    }
}
