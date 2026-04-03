using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Catalog.Application.Product.Queries.GetProducts;

/// <summary>List criteria for products: optional search, category and price filters, sort, and optional paging.</summary>
public sealed record GetProductsQuery(
    string? Search = null,
    List<Guid>? CategoryIds = null,
    decimal? PriceMin = null,
    decimal? PriceMax = null,
    ProductListSort Sort = ProductListSort.NameAsc,
    int? Page = null,
    int? PageSize = null) : IQuery<ProductListResult>;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<ProductListResult>>
{
    private readonly IProductReadRepository _repository;

    public GetProductsQueryHandler(IProductReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ProductListResult>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _repository.ListActiveProductsAsync(request, cancellationToken).ConfigureAwait(false);
        return Result<ProductListResult>.Success(list);
    }
}
