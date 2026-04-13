using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Catalog.Application.Product.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Uid) : IQuery<ProductResponse>;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    private readonly IProductReadRepository _productReadRepository;

    public GetProductByIdQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<Result<ProductResponse>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        ProductResponse? product = await _productReadRepository
            .GetActiveProductByUidAsync(request.Uid, cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.NotFound(ResultCodes.NotFound, "Product not found.");
        }

        return Result<ProductResponse>.Success(product);
    }
}
