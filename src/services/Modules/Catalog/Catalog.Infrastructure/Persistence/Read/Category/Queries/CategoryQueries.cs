using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using Catalog.Infrastructure.Persistence.Read.Category.Factories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Read.Category.Queries;

internal sealed class CategoryQueries : ICategoryReadRepository
{
    private readonly CatalogReadDbContext _db;
    private readonly CategoryReadFactory _readFactory;

    public CategoryQueries(CatalogReadDbContext db, CategoryReadFactory readFactory)
    {
        _db = db;
        _readFactory = readFactory;
    }

    public async Task<IReadOnlyList<CategoryResponse>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _db.Categories
            .Where(c => c.ParentId == null && c.DeletedOn == null)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows.Select(_readFactory.ToResponse).ToList();
    }

    public async Task<IReadOnlyList<CategoryMenuSourceRow>> GetAllActiveCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _db.Categories
            .Where(c => c.DeletedOn == null)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryMenuSourceRow(c.Id, c.Name, c.Slug, c.ParentId))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows;
    }
}
