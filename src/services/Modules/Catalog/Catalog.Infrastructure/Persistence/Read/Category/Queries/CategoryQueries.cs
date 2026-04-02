using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using Catalog.Infrastructure.Persistence.Read.Category.Factories;
using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Read.Category.Queries;

internal sealed class CategoryQueries(CatalogReadDbContext db, CategoryReadFactory readFactory)
    : ICategoryReadRepository
{
    public async Task<IReadOnlyList<CategoryResponse>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await db.Set<CategoryEntity>()
            .Where(c => c.ParentId == null && c.DeletedOn == null)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows.Select(readFactory.ToResponse).ToList();
    }

    public async Task<IReadOnlyList<CategoryMenuSourceRow>> GetAllActiveCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await db.Set<CategoryEntity>()
            .Where(c => c.DeletedOn == null)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryMenuSourceRow(c.Id, c.Name, c.Slug, c.ParentId))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows;
    }
}

