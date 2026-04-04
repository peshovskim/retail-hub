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
    private readonly ICartReadRepository _cartReadRepository;
    private readonly IProductReadRepository _productReadRepository;

    public GetCartQueryHandler(ICartReadRepository cartReadRepository, IProductReadRepository productReadRepository)
    {
        _cartReadRepository = cartReadRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<Result<CartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartReadRepository
            .GetByIdWithItemsAsync(request.CartId, cancellationToken)
            .ConfigureAwait(false);

        if (cart is null)
        {
            return Result<CartResponse>.Failure(Error.NotFound("Cart not found."));
        }

        CartResponse dto = await CartResponseFactory
            .CreateAsync(cart, _productReadRepository, cancellationToken)
            .ConfigureAwait(false);

        return Result<CartResponse>.Success(dto);
    }
}
