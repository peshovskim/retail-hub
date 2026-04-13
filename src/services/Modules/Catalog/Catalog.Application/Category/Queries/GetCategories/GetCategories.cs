using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Catalog.Application.Category.Queries.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyList<CategoryResponse>>;

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<IReadOnlyList<CategoryResponse>>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;

    public GetCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<Result<IReadOnlyList<CategoryResponse>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<CategoryResponse> responses = await _categoryReadRepository
            .GetRootCategoriesAsync(cancellationToken);

        return Result<IReadOnlyList<CategoryResponse>>.Success(responses);
    }
}
