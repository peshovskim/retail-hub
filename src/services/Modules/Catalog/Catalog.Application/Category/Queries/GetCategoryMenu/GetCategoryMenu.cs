using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Catalog.Application.Category.Queries.GetCategoryMenu;

public sealed record GetCategoryMenuQuery : IQuery<IReadOnlyList<CategoryMenuNodeResponse>>;

public sealed class GetCategoryMenuQueryHandler : IRequestHandler<GetCategoryMenuQuery, Result<IReadOnlyList<CategoryMenuNodeResponse>>>
{
    private readonly ICategoryReadRepository _categoryReadRepository;

    public GetCategoryMenuQueryHandler(ICategoryReadRepository categoryReadRepository)
    {
        _categoryReadRepository = categoryReadRepository;
    }

    public async Task<Result<IReadOnlyList<CategoryMenuNodeResponse>>> Handle(
        GetCategoryMenuQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<CategoryMenuSourceRow> rows = await _categoryReadRepository.GetAllActiveCategoriesAsync(cancellationToken);

        List<CategoryMenuSourceRow> orderedRows = rows.OrderBy(r => r.Name).ToList();
        ILookup<int?, CategoryMenuSourceRow> byParent = orderedRows.ToLookup(r => r.ParentId);

        return Result<IReadOnlyList<CategoryMenuNodeResponse>>.Success(BuildTree(byParent, null));
    }

    private static IReadOnlyList<CategoryMenuNodeResponse> BuildTree(
        ILookup<int?, CategoryMenuSourceRow> byParent,
        int? parentId)
    {
        List<CategoryMenuSourceRow> children = byParent[parentId].ToList();
        if (children.Count == 0)
        {
            return Array.Empty<CategoryMenuNodeResponse>();
        }

        List<CategoryMenuNodeResponse> nodes = new(children.Count);
        foreach (var row in children)
        {
            nodes.Add(new CategoryMenuNodeResponse(row.Id, row.Name, row.Slug, BuildTree(byParent, row.Id)));
        }

        return nodes;
    }
}
