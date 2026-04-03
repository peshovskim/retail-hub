using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Catalog.Application.Product.Queries.GetProducts;

public sealed record GetProductsQuery : IQuery<IReadOnlyList<ProductResponse>>;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IReadOnlyList<ProductResponse>>>
{
    private readonly IProductReadRepository _repository;

    public GetProductsQueryHandler(IProductReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<ProductResponse>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var responses = await _repository.GetAllActiveProductsAsync(cancellationToken).ConfigureAwait(false);
        return Result<IReadOnlyList<ProductResponse>>.Success(responses);
    }
}
