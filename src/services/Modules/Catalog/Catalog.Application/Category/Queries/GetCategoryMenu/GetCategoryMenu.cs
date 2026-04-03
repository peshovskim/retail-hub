using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Application.Common.Results;

namespace Catalog.Application.Category.Queries.GetCategoryMenu;

public sealed record GetCategoryMenuQuery : IQuery<IReadOnlyList<CategoryMenuNodeResponse>>;

public sealed class GetCategoryMenuQueryHandler : IRequestHandler<GetCategoryMenuQuery, Result<IReadOnlyList<CategoryMenuNodeResponse>>>
{
    private readonly ICategoryReadRepository _repository;

    public GetCategoryMenuQueryHandler(ICategoryReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<CategoryMenuNodeResponse>>> Handle(
        GetCategoryMenuQuery request,
        CancellationToken cancellationToken)
    {
        var rows = await _repository.GetAllActiveCategoriesAsync(cancellationToken).ConfigureAwait(false);

        var orderedRows = rows.OrderBy(r => r.Name).ToList();
        var byParent = orderedRows.ToLookup(r => r.ParentId);

        return Result<IReadOnlyList<CategoryMenuNodeResponse>>.Success(BuildTree(byParent, null));
    }

    private static IReadOnlyList<CategoryMenuNodeResponse> BuildTree(
        ILookup<Guid?, CategoryMenuSourceRow> byParent,
        Guid? parentId)
    {
        var children = byParent[parentId].ToList();
        if (children.Count == 0)
        {
            return Array.Empty<CategoryMenuNodeResponse>();
        }

        var nodes = new List<CategoryMenuNodeResponse>(children.Count);
        foreach (var row in children)
        {
            nodes.Add(new CategoryMenuNodeResponse(row.Id, row.Name, row.Slug, BuildTree(byParent, row.Id)));
        }

        return nodes;
    }
}
