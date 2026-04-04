using Cart.Application.Cart.Queries.GetCart;
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
}
