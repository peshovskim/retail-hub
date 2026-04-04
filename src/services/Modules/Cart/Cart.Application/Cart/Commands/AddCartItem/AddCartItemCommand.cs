using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Requests;
using Cart.Application.Cart.Responses;
using Cart.Application.Cart;
using Catalog.Application.Product.Interfaces;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Cart.Application.Cart.Commands.AddCartItem;

public sealed record AddCartItemCommand(Guid CartId, Guid ProductId, int Quantity) : ICommand<CartResponse>
{
    public AddCartItemCommand(AddCartItemRequest request)
        : this(request.CartId, request.ProductId, request.Quantity)
    {
    }
}

public sealed class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, Result<CartResponse>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductReadRepository _productReadRepository;

    public AddCartItemCommandHandler(ICartRepository cartRepository, IProductReadRepository productReadRepository)
    {
        _cartRepository = cartRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<Result<CartResponse>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdWithItemsAsync(request.CartId, cancellationToken).ConfigureAwait(false);

        if (cart is null)
        {
            return Result<CartResponse>.Failure(Error.NotFound("Cart not found."));
        }

        var product = await _productReadRepository
            .GetActiveProductByIdAsync(request.ProductId, cancellationToken)
            .ConfigureAwait(false);

        if (product is null)
        {
            return Result<CartResponse>.Failure(Error.NotFound("Product not found."));
        }

        var addResult = cart.AddOrUpdateItem(
            request.ProductId,
            request.Quantity,
            product.Price,
            DateTime.UtcNow,
            Guid.NewGuid);

        if (addResult.IsFailure)
        {
            return Result<CartResponse>.Failure(DomainResultAdapter.ToApplicationError(addResult.Error!));
        }

        await _cartRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        CartResponse dto = await CartResponseFactory.CreateAsync(cart, _productReadRepository, cancellationToken).ConfigureAwait(false);

        return Result<CartResponse>.Success(dto);
    }
}
