using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Catalog.Application.Product.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;

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
        var product = await _productReadRepository
            .GetActiveProductByIdAsync(request.Id, cancellationToken)
            .ConfigureAwait(false);

        if (product is null)
        {
            return Result<ProductResponse>.Failure(Error.NotFound("Product not found."));
        }

        return Result<ProductResponse>.Success(product);
    }
}
