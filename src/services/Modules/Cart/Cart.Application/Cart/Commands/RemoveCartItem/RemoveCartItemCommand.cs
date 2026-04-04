using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Requests;
using Cart.Application.Cart.Responses;
using Catalog.Application.Product.Interfaces;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Cart.Application.Cart.Commands.RemoveCartItem;

public sealed record RemoveCartItemCommand(Guid CartId, Guid ProductId) : ICommand<CartResponse>
{
    public RemoveCartItemCommand(RemoveCartItemRequest request, Guid productId)
        : this(request.CartId, productId)
    {
    }
}

public sealed class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, Result<CartResponse>>
{
    private readonly ICartRepository _carts;
    private readonly IProductReadRepository _products;

    public RemoveCartItemCommandHandler(ICartRepository carts, IProductReadRepository products)
    {
        _carts = carts;
        _products = products;
    }

    public async Task<Result<CartResponse>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _carts.GetByIdWithItemsAsync(request.CartId, cancellationToken).ConfigureAwait(false);

        if (cart is null)
        {
            return Result<CartResponse>.Failure(Error.NotFound("Cart not found."));
        }

        cart.RemoveItem(request.ProductId, DateTime.UtcNow);

        await _carts.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        CartResponse dto = await CartResponseFactory.CreateAsync(cart, _products, cancellationToken).ConfigureAwait(false);

        return Result<CartResponse>.Success(dto);
    }
}
