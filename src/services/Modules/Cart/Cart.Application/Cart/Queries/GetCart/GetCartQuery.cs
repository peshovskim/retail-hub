using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Responses;
using Catalog.Application.Product.Interfaces;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Cart.Application.Cart.Queries.GetCart;

public sealed record GetCartQuery(Guid CartId) : IQuery<CartResponse>;

public sealed class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<CartResponse>>
{
    private readonly ICartReadRepository _carts;
    private readonly IProductReadRepository _products;

    public GetCartQueryHandler(ICartReadRepository carts, IProductReadRepository products)
    {
        _carts = carts;
        _products = products;
    }

    public async Task<Result<CartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _carts
            .GetByIdWithItemsAsync(request.CartId, cancellationToken)
            .ConfigureAwait(false);

        if (cart is null)
        {
            return Result<CartResponse>.Failure(Error.NotFound("Cart not found."));
        }

        var dto = await CartResponseFactory
            .CreateAsync(cart, _products, cancellationToken)
            .ConfigureAwait(false);
        return Result<CartResponse>.Success(dto);
    }
}
