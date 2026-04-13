using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Requests;
using Cart.Application.Cart.Responses;
using Cart.Application.Cart;
using CartEntity = Cart.Domain.Cart.Domain.Cart;
using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

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
    private readonly ILogger<AddCartItemCommandHandler> _logger;

    public AddCartItemCommandHandler(
        ICartRepository cartRepository,
        IProductReadRepository productReadRepository,
        ILogger<AddCartItemCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _productReadRepository = productReadRepository;
        _logger = logger;
    }

    public async Task<Result<CartResponse>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
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

        Result addResult = cart.AddOrUpdateItem(
            product.ProductId,
            request.Quantity,
            product.Price,
            DateTime.UtcNow);

        if (addResult.IsFailure)
        {
            return Result.FromError<CartResponse>(addResult);
        }

        await _cartRepository.SaveChangesAsync(cancellationToken);

        CartResponse dto = await CartResponseFactory
            .CreateAsync(cart, _productReadRepository, cancellationToken);

        _logger.LogInformation(
            "Added product {ProductUid} x {Quantity} to cart {CartId}",
            request.ProductId,
            request.Quantity,
            request.CartId);

        return Result<CartResponse>.Success(dto);
    }
}
