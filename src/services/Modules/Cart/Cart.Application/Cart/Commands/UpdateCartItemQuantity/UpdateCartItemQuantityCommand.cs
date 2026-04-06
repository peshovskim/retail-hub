using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Requests;
using Cart.Application.Cart.Responses;
using Cart.Application.Cart;
using Catalog.Application.Product.Interfaces;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Cart.Application.Cart.Commands.UpdateCartItemQuantity;

public sealed record UpdateCartItemQuantityCommand(Guid CartId, Guid ProductId, int Quantity)
    : ICommand<CartResponse>
{
    public UpdateCartItemQuantityCommand(UpdateCartItemRequest request, Guid productId)
        : this(request.CartId, productId, request.Quantity)
    {
    }
}

public sealed class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, Result<CartResponse>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductReadRepository _productReadRepository;

    public UpdateCartItemQuantityCommandHandler(ICartRepository cartRepository, IProductReadRepository productReadRepository)
    {
        _cartRepository = cartRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<Result<CartResponse>> Handle(
        UpdateCartItemQuantityCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await _cartRepository
            .GetByIdWithItemsAsync(request.CartId, cancellationToken)
            .ConfigureAwait(false);

        if (cart is null)
        {
            return Result<CartResponse>.NotFound(ResultCodes.NotFound, "Cart not found.");
        }

        var product = await _productReadRepository
            .GetActiveProductByUidAsync(request.ProductId, cancellationToken)
            .ConfigureAwait(false);

        if (product is null)
        {
            return Result<CartResponse>.NotFound(ResultCodes.NotFound, "Product not found.");
        }

        if (request.Quantity == 0)
        {
            cart.RemoveItem(product.ProductId, DateTime.UtcNow);
        }
        else
        {
            var setResult = cart.SetItemQuantity(
                product.ProductId,
                request.Quantity,
                product.Price,
                DateTime.UtcNow);

            if (setResult.IsFailure)
            {
                return Result.FromError<CartResponse>(setResult);
            }
        }

        await _cartRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        CartResponse dto = await CartResponseFactory
            .CreateAsync(cart, _productReadRepository, cancellationToken)
            .ConfigureAwait(false);

        return Result<CartResponse>.Success(dto);
    }
}
