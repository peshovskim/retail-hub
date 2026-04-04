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
    public const string CartSessionHeaderName = "X-Cart-Session";

    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Creates a new anonymous cart or returns an existing one for the given session key.</summary>
    /// <param name="clientAnonymousKey">Optional client-generated key; omit to start a new session.</param>
    [HttpPost("session")]
    [ProducesResponseType(typeof(CartSessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrGetSession(
        [FromHeader(Name = CartSessionHeaderName)] string? clientAnonymousKey,
        CancellationToken cancellationToken)
    {
        Result<CartSessionResponse> result =
            await _mediator.Send(new CreateOrGetCartSessionCommand(clientAnonymousKey), cancellationToken)
                .ConfigureAwait(false);

        return result.ToActionResult();
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

    [HttpDelete("items/{productId:guid}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem(
        Guid productId,
        [FromQuery] RemoveCartItemRequest request,
        CancellationToken cancellationToken)
    {
        Result<CartResponse> result = await _mediator
            .Send(new RemoveCartItemCommand(request, productId), cancellationToken)
            .ConfigureAwait(false);

        return result.ToActionResult();
    }
}
