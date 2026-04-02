using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Catalog.Application.Category.Queries.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyList<CategoryResponse>>;

public sealed class GetCategoriesQueryHandler(ICategoryReadRepository repository)
    : IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryResponse>>>
{
    public async Task<Result<IReadOnlyList<CategoryResponse>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var responses = await repository.GetRootCategoriesAsync(cancellationToken).ConfigureAwait(false);
        return Result<IReadOnlyList<CategoryResponse>>.Success(responses);
    }
}

