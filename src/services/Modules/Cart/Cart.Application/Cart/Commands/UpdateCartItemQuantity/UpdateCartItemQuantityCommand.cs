using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Requests;
using Cart.Application.Cart.Responses;
using Cart.Application.Cart;
using Catalog.Application.Product.Interfaces;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

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
        var cart = await _cartRepository.GetByIdWithItemsAsync(request.CartId, cancellationToken).ConfigureAwait(false);

        if (cart is null)
        {
            return Result<CartResponse>.Failure(Error.NotFound("Cart not found."));
        }

        if (request.Quantity == 0)
        {
            cart.RemoveItem(request.ProductId, DateTime.UtcNow);
        }
        else
        {
            var product = await _productReadRepository
                .GetActiveProductByIdAsync(request.ProductId, cancellationToken)
                .ConfigureAwait(false);

            if (product is null)
            {
                return Result<CartResponse>.Failure(Error.NotFound("Product not found."));
            }

            var setResult = cart.SetItemQuantity(
                request.ProductId,
                request.Quantity,
                product.Price,
                DateTime.UtcNow);

            if (setResult.IsFailure)
            {
                return Result<CartResponse>.Failure(DomainResultAdapter.ToApplicationError(setResult.Error!));
            }
        }

        await _cartRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        CartResponse dto = await CartResponseFactory.CreateAsync(cart, _productReadRepository, cancellationToken).ConfigureAwait(false);

        return Result<CartResponse>.Success(dto);
    }
}
