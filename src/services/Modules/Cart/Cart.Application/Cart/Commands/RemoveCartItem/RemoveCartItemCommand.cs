using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Requests;
using Cart.Application.Cart.Responses;
using CartEntity = Cart.Domain.Cart.Domain.Cart;
using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

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
    private readonly ICartRepository _cartRepository;
    private readonly IProductReadRepository _productReadRepository;

    public RemoveCartItemCommandHandler(ICartRepository cartRepository, IProductReadRepository productReadRepository)
    {
        _cartRepository = cartRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<Result<CartResponse>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        CartEntity? cart = await _cartRepository
            .GetByIdWithItemsAsync(request.CartId, cancellationToken);

        if (cart is null)
        {
            return Result<CartResponse>.NotFound(ResultCodes.NotFound, "Cart not found.");
        }

        ProductResponse? product = await _productReadRepository
            .GetActiveProductByUidAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result<CartResponse>.NotFound(ResultCodes.NotFound, "Product not found.");
        }

        cart.RemoveItem(product.ProductId, DateTime.UtcNow);

        await _cartRepository.SaveChangesAsync(cancellationToken);

        CartResponse dto = await CartResponseFactory
            .CreateAsync(cart, _productReadRepository, cancellationToken);

        return Result<CartResponse>.Success(dto);
    }
}
